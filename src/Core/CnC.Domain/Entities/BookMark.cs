namespace CnC.Domain.Entities;

public class BookMark:BaseEntity
{
    public Guid ProductId { get; set; }

    public Product Product { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public AppUser User { get; set; } = null!;
}
