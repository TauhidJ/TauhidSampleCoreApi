using TauhidSampleCoreApi.Shared.Errors.ProductErrors;
using Zero.SeedWorks;
using Zero.SharedKernel.Types.Result;

namespace TauhidSampleCoreApi.Domain.Aggregates.ProductAggregate
{
    public class Product : Entity, IAggregateRoot
    {
        public int ProductId { get; private set; }
        public Name ProductName { get; private set; }
        public Quantity Quantity { get; private set; }
        public Price Price { get; private set; }
        public bool IsDeleted { get; private set; }

        /// <summary>
        /// Creates new product
        /// </summary>
        /// <param name="productName">Name of the product</param>
        /// <param name="quantity">Quantity of the product</param>
        /// <param name="price">Product of the price</param>
        public Product(Name productName, Quantity quantity, Price price)
        {
            ProductName = productName;
            Quantity = quantity;
            Price = price;
        }
        private Product()
        {
        }

        /// <summary>
        /// Updates product's details
        /// </summary>
        /// <param name="productName">Name of the product</param>
        /// <param name="quantity">Quantity of the product</param>
        /// <param name="price">Product of the price</param>
        public Result Update(Name productName, Quantity quantity, Price price)
        {
            if (IsDeleted) return Result.Failure(new DeletedProductError("Deleted product can not be updated."));

            ProductName = productName;
            Quantity = quantity;
            Price = price;

            return Result.Success();
        }
        /// <summary>
        /// Updates the product quantity when is created 
        /// </summary>
        /// <param name="quantity"></param>
        public void UpdateQuantity(Quantity quantity)
        {
            Quantity = quantity;
        }

        /// <summary>
        /// Deletes a product.
        /// </summary>
        public void Delete()
        {
            IsDeleted = true;
        }
    }
}
