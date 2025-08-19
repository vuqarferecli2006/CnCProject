namespace CnC.Application.DTOs;

public class EmailMessageDto
{
    public List<string> To { get; set; } = new();
    public string Subject { get; set; }=null!;
    public string Body { get; set; }=null!;
}
