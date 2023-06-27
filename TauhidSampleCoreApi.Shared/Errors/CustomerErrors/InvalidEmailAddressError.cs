using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TauhidSampleCoreApi.Shared.Errors.CustomerErrors
{
    public class InvalidEmailAddressError : CustomerError
    {
        public InvalidEmailAddressError(string message) : base(message)
        {
        }
    }
}
