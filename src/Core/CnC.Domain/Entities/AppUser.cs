using Microsoft.AspNetCore.Identity;

namespace CnC.Domain.Entities;

public class AppUser:IdentityUser
{
    public string FullName { get; set; } = null!;

    public string? ProfilePictureUrl { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime LastLoginDate { get; set; } = DateTime.UtcNow;

    public DateTime? RefreshTokenExpiryTime { get; set; }

    public Basket Basket { get; set; } = null!;

    public ICollection<Product> Products { get; set; } = new List<Product>();

    public ICollection<ProductView> ProductViews { get; set; } = new List<ProductView>();

    public ICollection<Order> Orders { get; set; } = new List<Order>();

    public ICollection<BookMark> BookMarks { get; set; } = new List<BookMark>();

    public ICollection<PaymentMethod> PaymentMethods { get; set; }= new List<PaymentMethod>();

}
