using Microsoft.AspNetCore.Mvc;
using TauhidSampleCoreApi.Applications.ModelFactory;
using TauhidSampleCoreApi.Domain.Aggregates.CustomerAggregate;
using TauhidSampleCoreApi.Domain.Aggregates.OrderAggregate;
using TauhidSampleCoreApi.Domain.Aggregates;
using TauhidSampleCoreApi.Domain.Specifications;
using TauhidSampleCoreApi.Shared.Errors.CustomerErrors;
using TauhidSampleCoreApi.Shared.Errors.OrderErrors;
using TauhidSampleCoreApi.Shared.Models.CustomerModels;
using Zero.SeedWorks;
using Zero.ServiceHelper;

namespace TauhidSampleCoreApi.Applications.Controllers
{
    [Route("customer")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Order> _orderRepository;
        public CustomerController(IRepository<Customer> customerRepository, IRepository<Order> orderRepository)
        {
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
        }

        /// <summary>
        /// Fetches list of all customers 
        /// </summary>
        /// <response code="200">Get list of customers</response>
        [ProducesResponseType(typeof(CustomerResponseModel), StatusCodes.Status200OK)]
        [HttpGet("~/customers")]
        public async Task<IActionResult> GetAllActiveCustomer()
        {
            var customers = await _customerRepository.ListAsync(new ActiveCustomerSpecification());

            return Ok(customers.Select(m => new CustomerResponseModel
            {
                EmailAddress = m.EmailAddress!.Value,
                MobileNumber = m.MobileNumber!.Value,
                Id = m.CustomerId,
                Name = m.Name,
            }));
        }

        /// <summary>
        /// Fetch the detail of customer by customer id. 
        /// </summary>
        /// <param name="customerId">Id of the customer</param>
        /// <response code="200">Get selected customer data</response>
        /// <response code="404">When the customer data is deleted or not exist</response>
        [ProducesResponseType(typeof(CustomerResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetAsyncByCustomerId(int customerId)
        {
            var customer = await _customerRepository.GetByIdAsync(customerId);
            if (customer == null || customer.IsDeleted) return NotFound();

            return Ok(new CustomerResponseModel
            {
                EmailAddress = customer.EmailAddress!.Value,
                MobileNumber = customer.MobileNumber!.Value,
                Id = customer.CustomerId,
                Name = customer.Name,
            });
        }

        /// <summary>
        /// Creates a customer
        /// </summary>
        /// <param name="model">Details of customer</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <response code="204">Customer added!</response>
        /// <response code="400">
        /// Errors:
        /// - <c><see cref="InvalidNameError"/></c>:Name is invalid.
        /// - <c><see cref="InvalidMobileNumberError"/></c>: Mobile number is invalid.
        /// - <c><see cref="InvalidEmailAddressError"/></c>:Email address is invalid.
        /// </response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<IActionResult> PostAsync(CustomerRequestModel model, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                bool isValid = true;
                var name = Name.Create(model.Name!);
                if (name.IsFailure)
                {
                    return this.ErrorProblem(new InvalidNameError("Name is invalid."));
                }
                var mobileNumber = MobileNumber.Create(model.MobileNumber, true);
                if (mobileNumber.IsFailure)
                {
                    return this.ErrorProblem(new InvalidMobileNumberError("Mobile number is invalid."));
                }
                var emailAddress = EmailAddress.Create(model.EmailAddress, true);
                if (emailAddress.IsFailure)
                {
                    return this.ErrorProblem(new InvalidEmailAddressError("Email address is invalid."));
                }
                if (isValid)
                {
                    var customer = new Customer(name.Value, mobileNumber.Value, emailAddress.Value);
                    await _customerRepository.AddAsync(customer);
                    await _customerRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

                    return NoContent();
                }
            }
            return ValidationProblem(ModelState);
        }

        /// <summary>
        /// Update the data of selected customer id
        /// </summary>
        /// <param name="customerId">Id of the customer </param>
        /// <param name="model">Details of the customer</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <response code="204">Updated data of the customer of selected id</response>
        /// <response code="400">
        /// Errors:
        /// - <c><see cref="InvalidNameError"/></c>:Name is invalid.
        /// - <c><see cref="InvalidMobileNumberError"/></c>: Mobile number is invalid.
        /// - <c><see cref="InvalidEmailAddressError"/></c>:Email address is invalid.
        /// </response>
        /// <response code="404">Customer data are deleted or not exist</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{customerId}")]
        public async Task<IActionResult> PutAsync(int customerId, CustomerRequestModel model, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var customer = await _customerRepository.GetByIdAsync(customerId);
                if (customer == null || customer.IsDeleted) return NotFound();
                bool isValid = true;
                var name = Name.Create(model.Name!);
                if (name.IsFailure)
                {
                    return this.ErrorProblem(new InvalidNameError("Name is invalid."));
                }
                var mobileNumber = MobileNumber.Create(model.MobileNumber, true);
                if (mobileNumber.IsFailure)
                {
                    return this.ErrorProblem(new InvalidMobileNumberError("Mobile number is invalid."));
                }
                var emailAddress = EmailAddress.Create(model.EmailAddress, true);
                if (emailAddress.IsFailure)
                {
                    return this.ErrorProblem(new InvalidEmailAddressError("Email address is invalid."));
                }
                if (isValid)
                {
                    var result = customer.Update(name.Value, mobileNumber.Value, emailAddress.Value);
                    if (result.IsSuccess)
                    {
                        await _customerRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
                        return NoContent();
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, result.Error.Message);
                    }
                }
            }
            return ValidationProblem(ModelState);
        }

        /// <summary>
        /// Fetches list of orders of a customer by customer id
        /// </summary>
        /// <param name="customerId">Id of the customer</param>
        /// <response code="200">Get list of orders of a customer by customer id</response>
        /// <response code="400">
        ///Errors:
        ///-<c><see cref="OrderNotFoundError"/></c>: If order not found.  
        /// </response>
        /// <response code="404">When the customer  is deleted or not exist</response>
        [ProducesResponseType(typeof(OrderResponseModelFactory), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{customerId}/orders")]
        public async Task<IActionResult> GetAllOrders(int customerId)
        {
            var customer = await _customerRepository.GetByIdAsync(customerId);
            if (customer == null) return NotFound();
            var orders = await _orderRepository.ListAllAsync();

            if (orders.Count == 0) { return this.ErrorProblem(new OrderNotFoundError()); }

            return Ok(orders.Where(x => x.CustomerId == customerId).Select(m => OrderResponseModelFactory.Create(m)));
        }

        /// <summary>
        /// Delete the  customer data 
        /// </summary>
        /// <param name="customerId">Id of the customer</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        ///<response code="404">When the customer's data is deleted or does not exist</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpDelete("{customerId}")]
        public async Task<IActionResult> DeleteAsync(int customerId, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(customerId);
            if (customer == null || customer.IsDeleted) return NotFound();
            customer.Delete();
            await _customerRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return NoContent();
        }
    }
}
