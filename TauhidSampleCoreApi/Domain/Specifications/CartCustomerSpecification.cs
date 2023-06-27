using TauhidSampleCoreApi.Domain.Aggregates.CartAggregate;
using Zero.SeedWorks;

namespace TauhidSampleCoreApi.Domain.Specifications
{
    public class CartCustomerSpecification : BaseSpecification<CartItem>
    {
        public CartCustomerSpecification(int customerId) : base(x => x.CustomerId == customerId)
        {
        }
    }
}
