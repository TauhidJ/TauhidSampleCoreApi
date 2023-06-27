using Microsoft.AspNetCore.Mvc;
using TauhidSampleCoreApi.Applications.ModelFactory;
using TauhidSampleCoreApi.Domain.Aggregates;
using TauhidSampleCoreApi.Domain.Aggregates.CartAggregate;
using TauhidSampleCoreApi.Domain.Aggregates.CustomerAggregate;
using TauhidSampleCoreApi.Domain.Aggregates.OrderAggregate;
using TauhidSampleCoreApi.Domain.Aggregates.ProductAggregate;
using TauhidSampleCoreApi.Domain.Specifications;
using TauhidSampleCoreApi.Shared.Errors.CartErrors;
using TauhidSampleCoreApi.Shared.Errors.OrderErrors;
using Zero.SeedWorks;
using Zero.ServiceHelper;

namespace TauhidSampleCoreApi.Applications.Controllers
{
    [Route("order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IRepository<CartItem> _cartRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Order> _orderRepository;
        public OrderController(IRepository<CartItem> shoppingCartRepository, IRepository<Product> productRepository, IRepository<Customer> customerRepository, IRepository<Order> orderRepository)
        {
            _cartRepository = shoppingCartRepository;
            _productRepository = productRepository;
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
        }
        /// <summary>
        /// Creates an order from the customer cart by customer Id.
        /// </summary>
        /// <param name="customerId">Id of the customer</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <response code="204">Order added!</response>
        /// <response code="400">
        ///Errors:
        ///-<c><see cref="CartIsEmptyError"/></c>: If cart is empty.  
        ///-<c><see cref="ProductOutOfStockError"/></c>: If any product is out of stock.  
        /// </response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("{customerId}")]
        public async Task<IActionResult> PostAsync(int customerId, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var customer = await _customerRepository.GetByIdAsync(customerId);
                if (customer == null) return NotFound();

                var cartItems = await _cartRepository.ListAsync(new CartCustomerSpecification(customerId));
                if (cartItems.Count == 0) { return this.ErrorProblem(new CartIsEmptyError()); }

                List<(Product product, Quantity quantity)> list1 = new();
                foreach (var item in cartItems)
                {
                    var product = await _productRepository.GetByIdAsync(item.ProductId);
                    list1.Add((product!, item.Quantity));
                }
                var order = Order.Create(customer, list1);
                if (order.IsFailure)
                {
                    return this.ErrorProblem(order.Error);

                }
                await _orderRepository.AddAsync(order.Value);
                await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

                return NoContent();
            }
            return ValidationProblem(ModelState);
        }
        /// <summary>
        /// Fetch the details of the order by order id. 
        /// </summary>
        /// <param name="orderId">Id of the order </param>
        /// <response code="200">Fetch selected order's detail.</response>
        /// <response code="404">When the order data is deleted or not exist</response>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(OrderResponseModelFactory), StatusCodes.Status200OK)]
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            var orderItem = await _orderRepository.GetByIdAsync(orderId);
            if (orderItem == null) { return NotFound(); }

            return Ok(OrderResponseModelFactory.Create(orderItem));
        }
        /// <summary>
        /// Delete the  order data 
        /// </summary>
        /// <param name="orderId">Id of the order</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <response code="400">
        ///Errors:
        ///-<c><see cref="OrderNotFoundError"/></c>: If order not found.  
        /// </response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{orderId}")]
        public async Task<IActionResult> DeleteAsync(int orderId, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) { return this.ErrorProblem(new OrderNotFoundError()); }
            _orderRepository.Delete(order);
            await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return NoContent();
        }

    }
}
