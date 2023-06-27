using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TauhidSampleCoreApi.Domain.Aggregates;
using TauhidSampleCoreApi.Domain.Aggregates.CustomerAggregate;

namespace TauhidSampleCoreApi.Infrastructure.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(x => x.CustomerId);

            builder.Property(x => x.Name)
                .HasConversion(x => x.Value, x => Name.Create(x).Value)
                .HasMaxLength(100)
                .IsUnicode(true)
                .IsRequired();

            builder.Property(m => m.MobileNumber)
                .HasConversion(m => m == null || m == MobileNumber.Empty ? null : m.Value, a => a == null ? null : (MobileNumber)a)
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(m => m.EmailAddress)
                .HasConversion(m => m == null || m == EmailAddress.Empty ? null : m.Value, a => a == null ? null : (EmailAddress)a)
                .HasMaxLength(100)
                .IsUnicode(true);
        }
    }
}
