using TABP.Domain.Entities;

namespace TABP.Application.Cities.Common;
public class CityResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Country { get; set; }
    public string PostOffice { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}