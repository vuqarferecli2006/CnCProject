using CnC.Application.Abstracts.Repositories.IProductDescriptionRepository;
using CnC.Application.Abstracts.Repositories.IProductFilesRepository;
using CnC.Application.Abstracts.Repositories.IProductRepositories;
using CnC.Application.Abstracts.Services;
using CnC.Application.Shared.Responses;
using CnC.Domain.Entities;
using MediatR;
using System.Net;

namespace CnC.Application.Features.ProductDescription.Commands.Update;

public class UpdateProductDescriptionHandler : IRequestHandler<UpdateProductDescriptionRequest, BaseResponse<string>>
{
    private readonly IProductDescriptionWriteRepository _productDescriptionWriteRepository;
    private readonly IProductDescriptionReadRepository _productDescriptionReadRepository;
    private readonly IProductReadRepository _productReadRepository;
    private readonly IFileServices _fileService;
    private readonly IProductFilesWriteRepository _productFilesWriteRepository;


    public UpdateProductDescriptionHandler(IProductDescriptionWriteRepository productDescriptionWriteRepository,
                                           IProductDescriptionReadRepository productDescriptionReadRepository,
                                           IFileServices fileService,
                                           IProductFilesWriteRepository productFilesWriteRepository,
                                           IProductReadRepository productReadRepository)
    {
        _productDescriptionWriteRepository = productDescriptionWriteRepository;
        _productDescriptionReadRepository = productDescriptionReadRepository;
        _fileService = fileService;
        _productFilesWriteRepository = productFilesWriteRepository;
        _productReadRepository = productReadRepository;
    }

    public async Task<BaseResponse<string>> Handle(UpdateProductDescriptionRequest request, CancellationToken cancellationToken)
    {
        var descriptionExists = await _productDescriptionReadRepository.GetByIdWithFilesAsync(request.DescriptionId, cancellationToken);
        if (descriptionExists is null)
            return new("Product description not found", HttpStatusCode.NotFound);

        var productExists = await _productReadRepository.GetByIdAsync(descriptionExists.ProductId);
        if (productExists is null)
            return new("Product not found", HttpStatusCode.NotFound);

        descriptionExists.Description = request.Description;
        descriptionExists.Model = request.Model;
        descriptionExists.UpdatedAt = DateTime.UtcNow;
        descriptionExists.DiscountedPercent = productExists.DiscountedPercent;

        if (request.ImageUrls is not null)
        {
            if (!string.IsNullOrEmpty(descriptionExists.ImageUrl))
            {
                await _fileService.DeleteFileAsync(descriptionExists.ImageUrl);
            }
            string imagePath = await _fileService.UploadAsync(request.ImageUrls, "product-descriptions");
            descriptionExists.ImageUrl = imagePath;
        }

        if (request.FileUrls is not null && request.FileUrls.Any())
        {
            var allExistingFiles = descriptionExists.ProductFiles.ToList();

            foreach (var existingFile in allExistingFiles)
            {
                await _fileService.DeleteFileAsync(existingFile.FileUrl);
                _productFilesWriteRepository.Delete(existingFile);
            }

            await _productFilesWriteRepository.SaveChangeAsync();
            foreach (var newFile in request.FileUrls)
            {
                string fileUrl = await _fileService.UploadAsync(newFile, "product-files");

                var productFile = new ProductFiles
                {
                    Id = Guid.NewGuid(),
                    ProductDescriptionId = descriptionExists.Id,
                    FileUrl = fileUrl,
                };
                await _productFilesWriteRepository.AddAsync(productFile);
            }

            await _productFilesWriteRepository.SaveChangeAsync();
        }

        _productDescriptionWriteRepository.Update(descriptionExists);
        await _productDescriptionWriteRepository.SaveChangeAsync();

        return new("Product description updated successfully", true, HttpStatusCode.OK);

    }
}
