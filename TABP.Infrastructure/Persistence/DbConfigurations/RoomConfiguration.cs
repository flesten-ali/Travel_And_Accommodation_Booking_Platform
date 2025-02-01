using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TABP.Domain.Entities;
namespace TABP.Infrastructure.Persistence.DbConfigurations;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
       builder.HasIndex(r=>r.RoomNumber).IsUnique();
    }
}
