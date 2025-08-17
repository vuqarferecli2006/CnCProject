namespace CnC.Domain.Entities;

public class Download:BaseEntity
{
    public string FileUrl { get; set; } = null!;

    public DateTime DownloadedAt { get; set; }

    public Guid OrderProductId { get; set; }    

    public OrderProduct OrderProduct { get; set; } = null!;
}
    