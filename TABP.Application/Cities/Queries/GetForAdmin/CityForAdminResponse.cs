using TABP.Domain.Entities;

namespace TABP.Application.Cities.Queries.GetForAdmin;
public class CityForAdminResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Country { get; set; }
    public string PostOffice { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}