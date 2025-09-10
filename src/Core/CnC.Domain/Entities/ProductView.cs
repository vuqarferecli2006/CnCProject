namespace CnC.Domain.Entities;

public class ProductView:BaseEntity
{
    public string? SessionId { get; set; }

    public DateTime ViewedAt { get; set; } = DateTime.UtcNow;

    public Guid ProductId { get; set; }

    public Product Product { get; set; } = null!;

    public string? UserId { get; set; } = null!;

    public AppUser User { get; set; } = null!;
}
