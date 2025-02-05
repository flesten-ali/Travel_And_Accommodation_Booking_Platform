using TABP.Domain.Entities;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Infrastructure.Persistence.DbContexts;

namespace TABP.Infrastructure.Persistence.Repositories;
public class BookingRepository(AppDbContext context) : Repository<Booking>(context), IBookingRepository
{
}
