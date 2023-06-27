using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TauhidSampleCoreApi.Shared.Errors.CustomerErrors
{
    public class InvalidMobileNumberError : CustomerError
    {
        public InvalidMobileNumberError(string message) : base(message)
        {

        }
    }
}
