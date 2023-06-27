using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TauhidSampleCoreApi.Domain.Aggregates;
using TauhidSampleCoreApi.Domain.Aggregates.CustomerAggregate;
using TauhidSampleCoreApi.Domain.Aggregates.OrderAggregate;
using TauhidSampleCoreApi.Domain.Aggregates.ProductAggregate;

namespace TauhidSampleCoreApi.Infrastructure.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.OrderId);
            builder.Property(x => x.OrderDate);

            builder.HasOne<Customer>()
             .WithMany()
             .HasForeignKey(m => m.CustomerId);

            builder.OwnsMany(m => m.Items, b =>
            {
                b.HasKey(x => x.OrderItemId);

                b.WithOwner()
                    .HasForeignKey(m => m.OrderId);

                b.HasOne<Product>()
                    .WithMany()
                    .HasForeignKey(x => x.ProductId);

                b.Property(m => m.Quantity)
                    .HasConversion(x => x.Value, x => Quantity.Create(x).Value);
            });
        }
    }
}
