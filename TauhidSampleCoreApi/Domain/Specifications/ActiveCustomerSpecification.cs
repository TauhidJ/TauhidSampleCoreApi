using TauhidSampleCoreApi.Domain.Aggregates.CustomerAggregate;
using Zero.SeedWorks;

namespace TauhidSampleCoreApi.Domain.Specifications
{
    public class ActiveCustomerSpecification : BaseSpecification<Customer>
    {
        public ActiveCustomerSpecification() : base(m => !m.IsDeleted)
        {
        }
    }
}
