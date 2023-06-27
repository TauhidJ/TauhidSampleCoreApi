using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TauhidSampleCoreApi.Shared.Errors.OrderErrors
{
    public class ProductOutOfStockError : OrderError
    {
        public ProductOutOfStockError(string name) : base($"{name} is out of stock.")
        {

        }
    }
}
