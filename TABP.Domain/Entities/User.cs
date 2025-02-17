using TABP.Domain.Common;
using TABP.Domain.Constants;
namespace TABP.Domain.Entities;

public class User : EntityBase<Guid>
{
    public User(string username, string email, string passwordHash)
    {
        UserName = username;
        Email = email;
        PasswordHash = passwordHash;
    }
    public User() { }

    public string UserName { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string Role { get; set; } = Roles.Guest;
    public ICollection<Review> Reviews { get; set; } = [];
    public ICollection<Booking> Bookings { get; set; } = [];
    public ICollection<CartItem> CartItems { get; set; } = [];
}