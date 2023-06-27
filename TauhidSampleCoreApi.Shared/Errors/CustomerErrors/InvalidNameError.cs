using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TauhidSampleCoreApi.Shared.Errors.CustomerErrors
{
    public class InvalidNameError : CustomerError
    {
        public InvalidNameError(string message) : base(message)
        {
        }
    }
}
