using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TauhidSampleCoreApi.Shared.Errors.CartErrors
{
    public class InvalidQuantityError : CartError
    {
        public InvalidQuantityError(string message) : base(message)
        {
        }
    }
}
