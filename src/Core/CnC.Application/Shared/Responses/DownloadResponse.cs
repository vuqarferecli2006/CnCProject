namespace CnC.Application.Shared.Responses;

public class DownloadResponse
{
    public Guid OrderId { get; set; }
    public bool IsPaid { get; set; }
    public DateTime OrderDate { get; set; }
    public List<DownloadProductResponse> Products { get; set; }=new List<DownloadProductResponse>();
}
