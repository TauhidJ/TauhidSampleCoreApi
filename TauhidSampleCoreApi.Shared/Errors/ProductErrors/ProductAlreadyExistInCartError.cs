using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TauhidSampleCoreApi.Shared.Errors.CartErrors;

namespace TauhidSampleCoreApi.Shared.Errors.ProductErrors
{
    public class ProductAlreadyExistInCartError : CartError
    {
        public ProductAlreadyExistInCartError() : base("Product already exists in cart.")
        {

        }
        public ProductAlreadyExistInCartError(string message) : base(message)
        {
        }
    }
}
