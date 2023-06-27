using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TauhidSampleCoreApi.Domain.Aggregates;
using TauhidSampleCoreApi.Domain.Aggregates.CartAggregate;
using TauhidSampleCoreApi.Domain.Aggregates.CustomerAggregate;
using TauhidSampleCoreApi.Domain.Aggregates.ProductAggregate;

namespace TauhidSampleCoreApi.Infrastructure.Configurations
{
    public class CartConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.HasKey(x => new {x.CustomerId, x.ProductId});

            builder.HasOne<Customer>()
              .WithMany()
              .HasForeignKey(m => m.CustomerId)
              .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Product>()
               .WithMany()
               .HasForeignKey(m => m.ProductId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.ProductName)
              .HasMaxLength(100)
              .IsUnicode(true)
              .IsRequired()
              .HasConversion(x => x.Value, x => Name.Create(x).Value);

            builder.Property(x => x.Quantity)
                 .HasConversion(x => x.Value, x => Quantity.Create(x).Value);


        }
    }
}
