using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TauhidSampleCoreApi.Shared.Errors.OrderErrors
{
    public class OrderNotFoundError : OrderError
    {
        public OrderNotFoundError() : base("Order does not exist.")
        {

        }
        public OrderNotFoundError(string message) : base(message)
        {
        }
    }
}
