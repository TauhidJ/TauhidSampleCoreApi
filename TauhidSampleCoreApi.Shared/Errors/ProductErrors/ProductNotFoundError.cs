using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TauhidSampleCoreApi.Shared.Errors.ProductErrors
{
    public class ProductNotFoundError : ProductError
    {
        public ProductNotFoundError() : base("Product doesn't exist.")
        {

        }
        public ProductNotFoundError(string message) : base(message)
        {
        }
    }
}
