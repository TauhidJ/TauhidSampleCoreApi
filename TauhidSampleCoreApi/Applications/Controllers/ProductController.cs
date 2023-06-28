using Microsoft.AspNetCore.Mvc;
using TauhidSampleCoreApi.Domain.Aggregates.ProductAggregate;
using TauhidSampleCoreApi.Domain.Aggregates;
using TauhidSampleCoreApi.Domain.Specifications;
using TauhidSampleCoreApi.Shared.Errors.ProductErrors;
using TauhidSampleCoreApi.Shared.Models.ProductModels;
using Zero.SeedWorks;
using Zero.ServiceHelper;

namespace TauhidSampleCoreApi.Applications.Controllers
{
    [Route("product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IRepository<Product> _productRepository;
        public ProductController(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        /// <summary>
        /// Fetches list of all products 
        /// </summary>
        /// <response code="200">Get list of all products</response>
        [ProducesResponseType(typeof(ProductResponseModel), StatusCodes.Status200OK)]
        [HttpGet("~/products")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productRepository.ListAsync(new ActiveProductSpecification());

            return Ok(products.Select(m => new ProductResponseModel
            {
                ProductId = m.ProductId,
                ProductName = m.ProductName,
                Quantity = m.Quantity,
                Price = m.Price
            }));
        }

        /// <summary>
        /// Fetch the details of the product by id. 
        /// </summary>
        /// <param name="productId">Id of the product</param>
        /// <response code="200">Get selected product data</response>
        /// <response code="404">When the product data is deleted or not exist</response>
        [ProducesResponseType(typeof(ProductResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{productId}")]
        public async Task<IActionResult> GetAsyncByProductId(int productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null || product.IsDeleted) return NotFound();

            return Ok(new ProductResponseModel
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Quantity = product.Quantity,
                Price = product.Price
            });
        }

        /// <summary>
        /// Creates a product
        /// </summary>
        /// <param name="model">Details of product</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <response code="204">Product added!</response>
        /// /// <response code="400">
        ///Errors:
        ///-<c><see cref="InvalidProductQuantityError"/></c>: If product quantity is invalid.  
        ///-<c><see cref="InvalidProductPriceError"/></c>: If product Price is invalid.
        ///-<c><see cref="InvalidProductNameError"/></c>: If product name is invalid.  
        /// </response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("/product")]
        public async Task<IActionResult> PostAsync(ProductRequestModel model, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var quantity = Quantity.Create(model.Quantity!.Value);
                if (quantity.IsFailure)
                {
                    return this.ErrorProblem(new InvalidProductQuantityError(quantity.Error.Message));
                }
                var price = Price.Create(model.Price!.Value);
                if (price.IsFailure)
                {
                    return this.ErrorProblem(new InvalidProductPriceError(price.Error.Message));
                }
                var name = Name.Create(model.ProductName);
                if (name.IsFailure)
                {
                    return this.ErrorProblem(new InvalidProductNameError(name.Error.Message));
                }

                var Product = new Product(name.Value, quantity.Value, price.Value);
                await _productRepository.AddAsync(Product);
                await _productRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
                return NoContent();

            }
            return ValidationProblem(ModelState);
        }

        /// <summary>
        /// Update data of selected product id
        /// </summary>
        /// <param name="productId">Id of the product</param>
        /// <param name="model">Details of product </param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <response code="200">Update data of the product by id</response>
        /// <response code="400">
        ///Errors:
        ///-<c><see cref="ProductNotFoundError"/></c>: If product not found.  
        ///-<c><see cref="InvalidProductNameError"/></c>: If product name is invalid.  
        /// </response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("{productId}")]
        public async Task<IActionResult> PutAsync(int productId, ProductRequestModel model, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var product = await _productRepository.GetByIdAsync(productId);
                if (product == null || product.IsDeleted) return this.ErrorProblem(new ProductNotFoundError());
                bool isValid = true;
                var productName = Name.Create(model.ProductName!);
                if (productName.IsFailure)
                {
                    return this.ErrorProblem(new InvalidProductNameError(productName.Error.Message));
                }
                var quantity = Quantity.Create(model.Quantity!.Value);
                if (quantity.IsFailure)
                {
                    return this.ErrorProblem(new InvalidProductQuantityError(quantity.Error.Message));
                }
                var price = Price.Create(model.Price!.Value);
                if (price.IsFailure)
                {
                    return this.ErrorProblem(new InvalidProductPriceError(price.Error.Message));
                }
                if (isValid)
                {
                    var result = product.Update(productName.Value, quantity.Value, price.Value);
                    if (result.IsFailure)
                    {
                        return this.ErrorProblem(result.Error);
                    }
                    await _productRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
                    return NoContent();
                }
            }
            return ValidationProblem(ModelState);
        }

        /// <summary>
        /// Delete the  product data by product id 
        /// </summary>
        /// <param name="productId">Id of the product</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <response code="204">Product deleted</response>
        /// <response code="404">When the product data is deleted or not exist</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteAsync(int productId, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null || product.IsDeleted) return NotFound();
            product.Delete();
            await _productRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return NoContent();
        }
    }
}
