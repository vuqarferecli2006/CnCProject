namespace CnC.Application.Shared.Responses;

public class InformationModelResponse
{
    public Guid Id { get; set; }
    public string Description { get; set; } = null!;
    public string VideoUrl { get; set; } = null!;
}
