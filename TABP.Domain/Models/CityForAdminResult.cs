namespace TABP.Domain.Models;

public sealed class CityForAdminResult
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Country { get; set; }
    public string PostOffice { get; set; }
    public int NumberOfHotels { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}