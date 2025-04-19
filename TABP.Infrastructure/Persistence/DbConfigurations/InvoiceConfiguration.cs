using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TABP.Domain.Entities;

namespace TABP.Infrastructure.Persistence.DbConfigurations;

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.Property(i => i.PaymentStatus).HasConversion<string>();
        builder.Property(i => i.TotalPrice).HasPrecision(18,2);
    }
}
