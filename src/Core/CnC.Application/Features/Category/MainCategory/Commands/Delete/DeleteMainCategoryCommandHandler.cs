using CnC.Application.Abstracts.Repositories.ICategoryRepositories;
using CnC.Application.Shared.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CnC.Application.Features.Category.MainCategory.Commands.Delete;

public class DeleteMainCategoryCommandHandler : IRequestHandler<DeleteMainCategoryCommandRequest, BaseResponse<string>>
{
    private readonly ICategoryReadRepository _categoryReadRepository;
    private readonly ICategoryWriteRepository _categoryWriteRepository;

    public DeleteMainCategoryCommandHandler(ICategoryReadRepository categoryReadRepository,
                                         ICategoryWriteRepository categoryWriteRepository)
    {
        _categoryReadRepository = categoryReadRepository;
        _categoryWriteRepository = categoryWriteRepository;
    }

    public async Task<BaseResponse<string>> Handle(DeleteMainCategoryCommandRequest request, CancellationToken cancellationToken)
    {
        var category=await _categoryReadRepository.GetByFiltered(
         c=>c.Id==request.Id && c.ParentCategory==null)
            .Include(c=>c.SubCategories.Where(sc=>!sc.IsDeleted))
            .Include(c=>c.Products.Where(p=>!p.IsDeleted))
            .FirstOrDefaultAsync(cancellationToken);

        if(category is null)
            return new("Main category not found",HttpStatusCode.NotFound);

        if(category.SubCategories.Any() || category.Products.Any())
            return new("Main category cannot be deleted because it contains subcategories or products. It must be empty.", HttpStatusCode.BadRequest);

        category.IsDeleted = true;
        _categoryWriteRepository.Update(category);
        await _categoryWriteRepository.SaveChangeAsync();

        return new("Main category deleted successfully",true,HttpStatusCode.OK);
    }
}
