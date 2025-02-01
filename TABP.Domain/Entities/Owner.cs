namespace TABP.Domain.Entities;

public class Owner : EntityBase<Guid>
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public ICollection<Hotel> Hotels { get; set; } = [];
}