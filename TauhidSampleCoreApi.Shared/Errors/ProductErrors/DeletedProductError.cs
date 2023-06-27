using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TauhidSampleCoreApi.Shared.Errors.ProductErrors
{
    public class DeletedProductError : ProductError
    {
        public DeletedProductError() : base("Product is deleted.")
        {

        }
        public DeletedProductError(string message) : base(message)
        {

        }
    }
}
