using TauhidSampleCoreApi.Domain.Aggregates.ProductAggregate;
using Zero.SeedWorks;

namespace TauhidSampleCoreApi.Domain.Specifications
{
    public class ActiveProductSpecification : BaseSpecification<Product>
    {
        public ActiveProductSpecification() : base(m => !m.IsDeleted)
        {
        }
    }
}
