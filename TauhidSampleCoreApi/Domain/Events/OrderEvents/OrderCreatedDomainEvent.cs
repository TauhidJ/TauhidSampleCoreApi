using TauhidSampleCoreApi.Domain.Aggregates.OrderAggregate;
using Zero.SeedWorks;

namespace TauhidSampleCoreApi.Domain.Events.OrderEvents
{
    public class OrderCreatedDomainEvent : DomainEvent
    {
        public Order Order { get; }

        public OrderCreatedDomainEvent(Order order)
        {
            Order = order;
        }
    }
}
