namespace CnC.Domain.Entities;

public class Basket:BaseEntity
{
    public string UserId { get; set; } = null!;

    public AppUser User { get; set; } = null!;

    public ICollection<ProductBasket> BasketItems { get; set; } = new List<ProductBasket>();
}
