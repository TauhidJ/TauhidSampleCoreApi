using TauhidSampleCoreApi.Domain.Aggregates.OrderAggregate;
using TauhidSampleCoreApi.Shared.Models.OrderModels;

namespace TauhidSampleCoreApi.Applications.ModelFactory
{
    public class OrderResponseModelFactory
    {
        public static OrderResponseModel Create(Order order)
        {
            return new OrderResponseModel
            {
                OrderId = order.OrderId,
                OrderDateTime = order.OrderDate,
                CustomerId = order.CustomerId,
                Items = order.Items.Select(x => new OrderResponseModel.ItemsResponseModel
                {
                    OrderItemId = x.OrderItemId,
                    ProductName = x.ProductName,
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    Rate = x.Rate,
                }).ToList(),
                TotalNumberOfItems = order.Items.Count(),
                TotalPrice = order.Items.Sum(x => x.Rate * x.Quantity),
            };
        }
    }
}
