namespace TABP.Application.Hotels.Queries.GetForAdmin;
public class HotelForAdminResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Rate { get; set; }
    public string OwnerName { get; set; }
    public string CityName { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
