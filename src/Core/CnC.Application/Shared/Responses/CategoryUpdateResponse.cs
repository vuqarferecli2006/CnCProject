namespace CnC.Application.Shared.Responses;

public class CategoryUpdateResponse
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public Guid? ParentCategoryId { get; set; }
}
