using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TauhidSampleCoreApi.Shared.Errors.ProductErrors
{
    public class InvalidProductNameError : ProductError
    {
        public InvalidProductNameError(string message) : base(message)
        {
        }
    }
}
