using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Zero.EFCoreSpecification;

namespace TauhidSampleCoreApi.Infrastructure
{
    public class ApplicationDbContext : DbContextBase<ApplicationDbContext>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IMediator mediator) : base(options, mediator) { }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
