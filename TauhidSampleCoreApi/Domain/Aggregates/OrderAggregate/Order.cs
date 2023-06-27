using TauhidSampleCoreApi.Domain.Aggregates.CustomerAggregate;
using TauhidSampleCoreApi.Domain.Aggregates.ProductAggregate;
using TauhidSampleCoreApi.Domain.Events.OrderEvents;
using TauhidSampleCoreApi.Shared.Errors.OrderErrors;
using Zero.SeedWorks;
using Zero.SharedKernel.Types.Result;

namespace TauhidSampleCoreApi.Domain.Aggregates.OrderAggregate
{
    public class Order : Entity, IAggregateRoot
    {
        public int OrderId { get; private set; }
        public DateTime OrderDate { get; private set; }
        public int CustomerId { get; private set; }

        private List<OrderItem> _items = new();
        public IReadOnlyList<OrderItem> Items => _items.AsReadOnly();

        private Order()
        {
        }

        /// <summary>
        ///  Creates new order
        /// </summary>
        /// <param name="model">Customer model</param>
        /// <param name="items"> List of items</param>
        private Order(Customer model, List<(Product product, Quantity quantity)> items)
        {
            foreach (var item in items)
            {
                _items.Add(new OrderItem(item.product.ProductId, item.product.ProductName, item.quantity, item.product.Price));
            }
            CustomerId = model.CustomerId;
            OrderDate = DateTime.UtcNow;
        }


        public static Result<Order> Create(Customer model, List<(Product product, Quantity quantity)> items)
        {
            foreach (var item in items)
            {
                if (item.quantity > item.product.Quantity.Value)
                {
                    return Result.Failure<Order>(new ProductOutOfStockError(item.product.ProductName));
                }
            }
            var order = new Order(model, items);

            order.AddDomainEvent(new OrderCreatedDomainEvent(order));
            return Result.Success(order);
        }
    }
}
