using CnC.Application.Abstracts.Repositories.ICategoryRepositories;
using CnC.Application.Shared.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CnC.Application.Features.Category.SubCategory.Commands.Delete;

public class DeleteSubCategoryCommandHandler : IRequestHandler<DeleteSubCategoryCommandRequest, BaseResponse<string>>
{
    private readonly ICategoryReadRepository _categoryReadRepository;
    private readonly ICategoryWriteRepository _categoryWriteRepository;

    public DeleteSubCategoryCommandHandler(ICategoryReadRepository categoryReadRepository, ICategoryWriteRepository categoryWriteRepository)
    {
        _categoryReadRepository = categoryReadRepository;
        _categoryWriteRepository = categoryWriteRepository;
    }

    public async Task<BaseResponse<string>> Handle(DeleteSubCategoryCommandRequest request, CancellationToken cancellationToken)
    {
        var category= await _categoryReadRepository.GetByFiltered(
            c=> c.Id == request.Id && c.ParentCategory!=null)
            .Include(c=> c.Products.Where(p=>!p.IsDeleted))
            .FirstOrDefaultAsync(cancellationToken);

        if (category is null)
            return new("Category not found", HttpStatusCode.NotFound);

        if (category.Products.Any())
            return new("Subcategory cannot be deleted because it contains products.", HttpStatusCode.BadRequest);

        category.IsDeleted = true;
        _categoryWriteRepository.Update(category);
        await _categoryWriteRepository.SaveChangeAsync();

        return new("Category deleted successfully",true, HttpStatusCode.OK);
    }
}
