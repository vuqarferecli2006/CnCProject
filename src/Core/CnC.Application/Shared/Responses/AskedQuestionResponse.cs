namespace CnC.Application.Shared.Responses;

public class AskedQuestionResponse
{
    public Guid Id { get; set; }
    public string VideoUrl { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Question { get; set; } = null!;
    public string Answer { get; set; } = null!;
}