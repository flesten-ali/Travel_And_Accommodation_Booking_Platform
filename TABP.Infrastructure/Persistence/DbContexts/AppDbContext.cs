using Microsoft.EntityFrameworkCore;
using TABP.Domain.Entities;

namespace TABP.Infrastructure.Persistence.DbContexts;

public class AppDbContext : DbContext
{
    public DbSet<Amenity> Amenities { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Owner> Owners { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<RoomClass> RoomClasses { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Invoice> Invoices { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AssemblyReference).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}