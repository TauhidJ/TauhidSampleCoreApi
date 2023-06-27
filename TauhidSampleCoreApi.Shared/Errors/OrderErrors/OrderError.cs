using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zero.SharedKernel.Types.Result;

namespace TauhidSampleCoreApi.Shared.Errors.OrderErrors
{
    public class OrderError : Error
    {
        public OrderError(string message) : base(message)
        {

        }
    }
}
