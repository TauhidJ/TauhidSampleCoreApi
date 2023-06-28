using MediatR;
using TauhidSampleCoreApi.Domain.Aggregates.ProductAggregate;
using TauhidSampleCoreApi.Domain.Aggregates;
using TauhidSampleCoreApi.Domain.Events.OrderEvents;
using Zero.SeedWorks;

namespace TauhidSampleCoreApi.Applications.DomainEventHandlers.OrderCreated
{
    public class UpdateProductQuantityWhenOrderCreatedDomainEventHandler : INotificationHandler<OrderCreatedDomainEvent>
    {
        private readonly IRepository<Product> _productRepository;

        public UpdateProductQuantityWhenOrderCreatedDomainEventHandler(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task Handle(OrderCreatedDomainEvent @event, CancellationToken cancellationToken)
        {
            var orderItems = @event.Order.Items.Select(x => x).ToList();

            foreach (var orderItem in orderItems)
            {
                var product = await _productRepository.GetByIdAsync(orderItem.ProductId);
                var newQuantity = product!.Quantity.Value - orderItem.Quantity.Value;

                product.UpdateQuantity((Quantity)newQuantity);
                await _productRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            }

        }
    }
}
