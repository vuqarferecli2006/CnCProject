using CnC.Application.Abstracts.Repositories.ICategoryRepositories;
using CnC.Application.Shared.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CnC.Application.Features.Category.MainCategory.Commands.Create;

public class CreateMainCategoryCommandHandler : IRequestHandler<CreateMainCategoryCommandRequest, BaseResponse<string>>
{
    private readonly ICategoryWriteRepository _categoryWriteRepository;
    private readonly ICategoryReadRepository _categoryReadRepository;

    public CreateMainCategoryCommandHandler(ICategoryWriteRepository categoryWriteRepository, ICategoryReadRepository categoryReadRepository)
    {
        _categoryWriteRepository = categoryWriteRepository;
        _categoryReadRepository = categoryReadRepository;
    }

    public async Task<BaseResponse<string>> Handle(CreateMainCategoryCommandRequest request, CancellationToken cancellationToken)
    {
        var existsCategory = await _categoryReadRepository
            .GetByFiltered(c => c.ParentCategoryId == null &&
                                c.Name.Trim().ToLower() == request.Name.Trim().ToLower())
            .AnyAsync(cancellationToken);

        if (existsCategory)
            return new("This main category already exists", false, HttpStatusCode.BadRequest);

        var category = new Domain.Entities.Category
        {
            Name = request.Name.Trim(),
            Description = request.Description.Trim(),
            ParentCategoryId = null
        };

        await _categoryWriteRepository.AddAsync(category);
        await _categoryWriteRepository.SaveChangeAsync();

        return new("Main category created successfully", true, HttpStatusCode.Created);
    }
}
