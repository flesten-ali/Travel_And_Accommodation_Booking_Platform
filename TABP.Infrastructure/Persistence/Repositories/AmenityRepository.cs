using TABP.Domain.Entities;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Infrastructure.Persistence.DbContexts;

namespace TABP.Infrastructure.Persistence.Repositories;
public class AmenityRepository(AppDbContext context) : Repository<Amenity>(context), IAmenityRepository

{
}
