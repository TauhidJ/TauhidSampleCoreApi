

namespace TauhidSampleCoreApi.Shared.Errors.CustomerErrors
{
    public class DeletedCustomerError : CustomerError
    {
        public DeletedCustomerError() : base("Customer is deleted.")
        {

        }
        public DeletedCustomerError(string message) : base(message)
        {

        }
    }
}
