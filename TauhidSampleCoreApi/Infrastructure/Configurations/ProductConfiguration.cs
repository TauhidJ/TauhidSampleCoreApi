using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TauhidSampleCoreApi.Domain.Aggregates;
using TauhidSampleCoreApi.Domain.Aggregates.ProductAggregate;

namespace TauhidSampleCoreApi.Infrastructure.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.ProductId);

            builder.Property(x => x.ProductName)
               .HasConversion(x => x.Value, x => Name.Create(x).Value)
               .HasMaxLength(100)
               .IsUnicode(true)
               .IsRequired();

            builder.Property(m => m.Quantity)
                .HasConversion(x => x.Value, x => Quantity.Create(x).Value);

            builder.Property(m => m.Price)
                .HasConversion(x => x.Value, x => Price.Create(x).Value);
        }
    }
}
