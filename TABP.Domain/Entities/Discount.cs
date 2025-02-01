namespace TABP.Domain.Entities;

public class Discount : EntityBase<Guid>
{
    public double Percentage { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public RoomClass RoomClass { get; set; }
    public Guid RoomClassId { get; set; }
}
