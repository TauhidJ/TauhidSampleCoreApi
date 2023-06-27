using Zero.EFCoreSpecification;
using Zero.SeedWorks;

namespace TauhidSampleCoreApi.Infrastructure.Repositories
{
    public class EfRepository<TEntity> : RepositoryBase<TEntity, ApplicationDbContext> where TEntity : Entity, IAggregateRoot
    {
        public EfRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
