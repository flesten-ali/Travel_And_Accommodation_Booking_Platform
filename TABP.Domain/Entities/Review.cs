namespace TABP.Domain.Entities;

public class Review : EntityBase<Guid>
{
    public string Comment { get; set; }
    public int Rate { get; set; }
    public DateTime CreatedAt { get; set; }
    public Hotel Hotel { get; set; }
    public Guid HotelId { get; set; }
    public User User { get; set; }
    public Guid UserId { get; set; }
}
