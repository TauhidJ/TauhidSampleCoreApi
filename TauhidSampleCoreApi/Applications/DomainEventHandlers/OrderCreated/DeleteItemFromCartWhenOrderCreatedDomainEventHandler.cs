using MediatR;
using TauhidSampleCoreApi.Domain.Aggregates.CartAggregate;
using TauhidSampleCoreApi.Domain.Events.OrderEvents;
using Zero.SeedWorks;

namespace TauhidSampleCoreApi.Applications.DomainEventHandlers.OrderCreated
{
    public class DeleteItemFromCartWhenOrderCreatedDomainEventHandler : INotificationHandler<OrderCreatedDomainEvent>
    {
        private readonly IRepository<CartItem> _cartItemRepository;
        public DeleteItemFromCartWhenOrderCreatedDomainEventHandler(IRepository<CartItem> cartItemRepository)
        {
            _cartItemRepository = cartItemRepository;
        }
        public async Task Handle(OrderCreatedDomainEvent @event, CancellationToken cancellationToken)
        {
            var orderItems = @event.Order.Items.ToList();
            var cart = await _cartItemRepository.ListAllAsync();
            foreach (var orderItem in orderItems)
            {
                var cartItem = cart.Where(x => x.CustomerId == @event.Order.CustomerId && x.ProductId == orderItem.ProductId).First();
                _cartItemRepository.Delete(cartItem);

            }
            await _cartItemRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        }
    }
}
