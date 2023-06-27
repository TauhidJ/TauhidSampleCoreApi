using Microsoft.AspNetCore.Mvc;
using TauhidSampleCoreApi.Domain.Aggregates;
using TauhidSampleCoreApi.Domain.Aggregates.CartAggregate;
using TauhidSampleCoreApi.Domain.Aggregates.CustomerAggregate;
using TauhidSampleCoreApi.Domain.Aggregates.ProductAggregate;
using TauhidSampleCoreApi.Domain.Specifications;
using TauhidSampleCoreApi.Shared.Errors.CartErrors;
using TauhidSampleCoreApi.Shared.Errors.ProductErrors;
using TauhidSampleCoreApi.Shared.Models.CartModels;
using Zero.SeedWorks;
using Zero.ServiceHelper;

namespace TauhidSampleCoreApi.Applications.Controllers
{
    [Route("cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IRepository<CartItem> _cartRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Product> _productRepository;
        public CartController(IRepository<CartItem> shoppingCartRepository, IRepository<Customer> customerRepository, IRepository<Product> productRepository)
        {
            _cartRepository = shoppingCartRepository;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
        }

        /// <summary>
        /// Fetches list of cart items of a customer
        /// </summary>
        /// <param name="customerId">Id of the customer</param>
        /// <response code="200">Get list of cart items by  customer id</response>
        /// <response code="404">When the customer does not exist</response>
        [ProducesResponseType(typeof(CartResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetCartItemListByCustomerId(int customerId)
        {
            var customer = await _customerRepository.GetByIdAsync(customerId);
            if (customer == null) return NotFound();
            var customerCart = await _cartRepository.ListAsync(new CartCustomerSpecification(customerId));
            return Ok(customerCart.Select(m => new CartResponseModel
            {
                CustomerId = m.CustomerId,
                ProductId = m.ProductId,
                ProductName = m.ProductName,
                Quantity = m.Quantity,
            }));
        }

        /// <summary>
        /// Add items to the customer's cart
        /// </summary>
        /// <param name="customerId">Id of the customer</param>
        /// <param name="model">Detail of cart</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <response code="204">Item added!</response>
        /// <response code="400">
        ///Errors:
        ///-<c><see cref="ProductNotFoundError"/></c>: If product not found.
        ///-<c><see cref="InvalidQuantityError"/></c>: If product quantity is invalid.
        ///-<c><see cref="ProductQuantityExceedError"/></c>: If product quantity exceed.
        /// </response> 
        /// <response code="404">When the customer data is deleted or not exist</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("{customerId}")]
        public async Task<IActionResult> PostAsync(int customerId, CartRequestModel model, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                bool isValid = true;
                var customer = await _customerRepository.GetByIdAsync(customerId);
                if (customer == null || customer.IsDeleted) { return NotFound(); }

                var product = await _productRepository.GetByIdAsync(model.ProductId);
                if (product == null || product.IsDeleted) return this.ErrorProblem(new ProductNotFoundError());

                var quantity = Quantity.Create(model.Quantity!);
                if (quantity.IsFailure)
                {
                    return this.ErrorProblem(new InvalidQuantityError(quantity.Error.Message));
                }
                var result = CartItem.CanAddProductQuantity(quantity.Value, product);
                if (result.IsFailure)
                {
                    return this.ErrorProblem(result.Error);
                }
                if (isValid)
                {
                    var cart = await _cartRepository.GetByIdAsync(customerId, model.ProductId);
                    if (cart == null)
                    {
                        var newCartItem = new CartItem(customer, product, quantity.Value);
                        await _cartRepository.AddAsync(newCartItem);
                    }
                    else
                    {
                        cart.UpdateQuantity(quantity.Value);
                    }
                    await _cartRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

                    return NoContent();
                }
            }
            return ValidationProblem(ModelState);
        }

        /// <summary>
        /// Delete items from cart of a customer by customer id
        /// </summary>
        /// <param name="customerId">Id of the customer</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <response code="400">
        ///Errors:
        ///-<c><see cref="CartIsEmptyError"/></c>: If cart is empty.  
        /// </response>
        /// <response code="404">Customer data does not exist</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{customerId}")]
        public async Task<IActionResult> DeleteAsync(int customerId, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(customerId);
            if (customer == null) return NotFound();
            var cartItems = await _cartRepository.ListAsync(new CartCustomerSpecification(customerId));
            if (cartItems.Count == 0) { return this.ErrorProblem(new CartIsEmptyError()); }
            foreach (var item in cartItems)
            {
                _cartRepository.Delete(item);
            }
            await _cartRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Delete single item from the cart   
        /// </summary>
        /// <param name="customerId">Id of the customer</param>
        /// <param name="productId">Id of the product</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <response code="400">
        ///Errors:
        ///-<c><see cref="ProductNotFoundError"/></c>: If product not found.  
        ///-<c><see cref="CartIsEmptyError"/></c>: If cart is empty.  
        /// </response>
        /// <response code="404">Customer data does not exist</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{customerId}/{productId}")]
        public async Task<IActionResult> DeleteSingleProductAsync(int customerId, int productId, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(customerId);
            if (customer == null || customer.IsDeleted) return NotFound();

            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null) return this.ErrorProblem(new ProductNotFoundError());

            var cartItem = await _cartRepository.GetByIdAsync(customerId, productId);
            if (cartItem == null) { return this.ErrorProblem(new CartIsEmptyError()); }
            _cartRepository.Delete(cartItem);
            await _cartRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return NoContent();
        }
    }

}
