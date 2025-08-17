namespace CnC.Domain.Entities;

public class FreequentlyAskedQuestion:BaseEntity
{
    public string VideoUrl { get; set; } = null!;

    public string Description { get; set; } = null!;
    
    public string Question { get; set; } = null!;

    public string Answer { get; set; } = null!;
}
