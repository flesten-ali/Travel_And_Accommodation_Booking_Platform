using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TABP.Domain.Entities;

namespace TABP.Infrastructure.Persistence.DbConfigurations;
public class RoomClassConfiguration : IEntityTypeConfiguration<RoomClass>
{
    public void Configure(EntityTypeBuilder<RoomClass> builder)
    {
        builder.Property(r => r.RoomType).HasConversion<string>();
    }
}
