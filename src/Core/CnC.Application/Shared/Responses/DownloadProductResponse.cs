namespace CnC.Application.Shared.Responses;

public class DownloadProductResponse
{
    public Guid ProductId {  get; set; }
    public string ProductName { get; set; } = null!;
    public List<string> FileUrl { get; set; }=new List<string>();
}
