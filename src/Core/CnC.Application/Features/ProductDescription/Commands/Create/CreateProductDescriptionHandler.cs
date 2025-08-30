using CnC.Application.Abstracts.Repositories.IProductDescriptionRepository;
using CnC.Application.Abstracts.Repositories.IProductFilesRepository;
using CnC.Application.Abstracts.Repositories.IProductRepositories;
using CnC.Application.Abstracts.Services;
using CnC.Application.Shared.Responses;
using CnC.Domain.Entities;
using MediatR;
using Microsoft.Extensions.FileProviders;
using System.Net;

namespace CnC.Application.Features.ProductDescription.Commands.Create;

public class CreateProductDescriptionHandler : IRequestHandler<CreateProductDescriptionRequest, BaseResponse<string>>
{
    private readonly IProductDescriptionWriteRepository _productDescriptionWriteRepository;
    private readonly IProductReadRepository _productReadRepository;
    private readonly IProductFilesWriteRepository _productFilesWriteRepository;
    private readonly IFileServices _fileService;

    public CreateProductDescriptionHandler(IProductDescriptionWriteRepository productDescriptionWriteRepository,
                                        IProductReadRepository productReadRepository,
                                        IFileServices fileServices,
                                        IProductFilesWriteRepository productFilesWriteRepository)
    {
        _productReadRepository = productReadRepository;
        _productDescriptionWriteRepository = productDescriptionWriteRepository;
        _fileService = fileServices;
        _productFilesWriteRepository = productFilesWriteRepository;
    }

    public async Task<BaseResponse<string>> Handle(CreateProductDescriptionRequest request, CancellationToken cancellationToken)
    {
        var productExist = await _productReadRepository.GetByIdAsync(request.ProductId);

        if(productExist is null)
            return new("Product not found", HttpStatusCode.NotFound);




        string imagePath = await _fileService.UploadAsync(request.ImageUrl, "product-descriptions");

        var description = new Domain.Entities.ProductDescription
        {
            ProductId = request.ProductId,
            Description = request.Description,
            Model = request.Model,
            CreatedAt = DateTime.UtcNow,
            DiscountedPercent=productExist.DiscountedPercent,
            ImageUrl = imagePath,
            
        };

        await _productDescriptionWriteRepository.AddAsync(description);
        await _productDescriptionWriteRepository.SaveChangeAsync();

        foreach (var file in request.FileUrl)
        {
            string fileUrl = await _fileService.UploadAsync(file, "product-files");

            var productFile = new ProductFiles
            {
                Id = Guid.NewGuid(),
                ProductDescriptionId = description.Id,
                FileUrl = fileUrl,
                CreatedAt = DateTime.UtcNow
            };

            await _productFilesWriteRepository.AddAsync(productFile);
        }

        await _productDescriptionWriteRepository.SaveChangeAsync();
        
        return new("Product description created successfully",true, HttpStatusCode.Created);
    }
}


