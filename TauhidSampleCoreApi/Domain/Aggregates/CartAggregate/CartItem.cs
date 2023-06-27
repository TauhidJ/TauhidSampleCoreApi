using TauhidSampleCoreApi.Domain.Aggregates.CustomerAggregate;
using TauhidSampleCoreApi.Domain.Aggregates.ProductAggregate;
using Zero.SeedWorks;
using Zero.SharedKernel.Types.Result;

namespace TauhidSampleCoreApi.Domain.Aggregates.CartAggregate
{
    public class CartItem : Entity, IAggregateRoot
    {
        public int CustomerId { get; private set; }
        public int ProductId { get; private set; }
        public Name ProductName { get; private set; }
        public Quantity Quantity { get; private set; }

        private CartItem()
        {
        }

        /// <summary>
        /// Add products in the cart
        /// </summary>
        /// <param name="customer">Detail of customer</param>
        /// <param name="product">Detail of product</param>
        /// <param name="quantity">Quantity </param>
        public CartItem(Customer customer, Product product, Quantity quantity)
        {
            CustomerId = customer.CustomerId;
            ProductId = product.ProductId;
            ProductName = product.ProductName;
            Quantity = quantity;
        }

        /// <summary>
        /// Updates the cart quantity when product is already in  the cart 
        /// </summary>
        /// <param name="quantity">Quantity</param>
        public void UpdateQuantity(Quantity quantity)
        {
            Quantity = quantity;
        }

        /// <summary>
        /// Checks whether product quantity can be added to the cart.
        /// </summary>
        /// <param name="quantity">Cart quantity </param>
        /// <param name="product">Product</param>
        public static Result CanAddProductQuantity(Quantity quantity, Product product)
        {
            if (quantity.Value > product.Quantity)
            {
                return Result.Failure(new ProductQuantityExceedError());
            }
            return Result.Success();
        }
    }
}
