using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TauhidSampleCoreApi.Shared.Errors.ProductErrors
{
    public class InvalidProductQuantityError : ProductError
    {
        public InvalidProductQuantityError(string message) : base(message)
        {
        }
    }
}
