namespace CnC.Application.Shared.Responses;

public class CategorySubResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public List<CategorySubResponse>? SubCategories { get; set; }
}
