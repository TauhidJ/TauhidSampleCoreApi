
using Zero.SharedKernel.Types.Result;

namespace TauhidSampleCoreApi.Shared.Errors.CustomerErrors
{
    public class CustomerError : Error
    {
        public CustomerError(string message) : base(message)
        {

        }
    }
}
