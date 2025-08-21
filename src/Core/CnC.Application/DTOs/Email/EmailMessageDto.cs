namespace CnC.Application.DTOs.Email;

public record EmailMessageDto
{
    public List<string> To { get; set; } = new();
    public string Subject { get; set; }=null!;
    public string Body { get; set; }=null!;
}
