namespace CnC.Domain.Entities;

public class InformationModel:BaseEntity
{
    public string Description { get; set; } = null!;

    public string VideoUrl { get; set; } = null!;
}
