using Zero.SeedWorks;

namespace TauhidSampleCoreApi.Domain.Aggregates.OrderAggregate
{
    public class OrderItem : Entity
    {
        public int OrderItemId { get; private set; }
        public string ProductName { get; private set; }
        public int ProductId { get; private set; }
        public Quantity Quantity { get; private set; }
        public int Rate { get; private set; }
        public int OrderId { get; private set; }

        private OrderItem()
        {
        }
        /// <summary>
        /// creates new order items
        /// </summary>
        /// <param name="productId">Id of the product</param>
        /// <param name="productName">Name of the product</param>
        /// <param name="quantity">Quantity of the product</param>
        /// <param name="price">Rate of the product</param>
        public OrderItem(int productId, string productName, Quantity quantity, int price)
        {
            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
            Rate = price;
        }
    }
}
