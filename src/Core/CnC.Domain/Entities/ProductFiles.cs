namespace CnC.Domain.Entities;

public class ProductFiles: BaseEntity
{
    public string FileUrl { get; set; } = null!;

    public Guid ProductDescriptionId { get; set; }

    public ProductDescription ProductDescription { get; set; } = null!;

    public ICollection<Download> Downloads { get; set; } = new List<Download>();
}
