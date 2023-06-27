using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TauhidSampleCoreApi.Shared.Errors.OrderErrors;

namespace TauhidSampleCoreApi.Shared.Errors.CartErrors
{
    public class CartIsEmptyError : OrderError
    {
        public CartIsEmptyError() : base("Cart is empty.")
        {

        }
        public CartIsEmptyError(string message) : base(message)
        {
        }
    }
}
