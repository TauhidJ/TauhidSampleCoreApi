using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TauhidSampleCoreApi.Shared.Models.OrderModels
{
    public class OrderResponseModel
    {
        public int OrderId { get; set; }
        public DateTime OrderDateTime { get; set; }
        public int CustomerId { get; set; }
        public List<ItemsResponseModel> Items { get; set; }
        public int TotalNumberOfItems { get; set; }
        public int TotalPrice { get; set; }
        public class ItemsResponseModel
        {
            public int OrderItemId { get; set; }
            public string ProductName { get; set; }
            public int ProductId { get; set; }
            public int Quantity { get; set; }
            public int Rate { get; set; }
        }
    }
}
