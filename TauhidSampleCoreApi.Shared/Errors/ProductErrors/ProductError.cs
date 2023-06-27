using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zero.SharedKernel.Types.Result;

namespace TauhidSampleCoreApi.Shared.Errors.ProductErrors
{
    public class ProductError : Error
    {
        public ProductError(string message) : base(message)
        {

        }
    }
}
