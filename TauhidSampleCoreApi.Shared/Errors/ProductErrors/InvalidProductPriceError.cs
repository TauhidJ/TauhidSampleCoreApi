using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TauhidSampleCoreApi.Shared.Errors.ProductErrors
{
    public class InvalidProductPriceError : ProductError
    {
        public InvalidProductPriceError(string message) : base(message)
        {
        }
    }
}
