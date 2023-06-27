using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zero.SharedKernel.Types.Result;

namespace TauhidSampleCoreApi.Shared.Errors.CartErrors
{
    public class CartError : Error
    {
        public CartError(string message) : base(message)
        {

        }
    }
}
