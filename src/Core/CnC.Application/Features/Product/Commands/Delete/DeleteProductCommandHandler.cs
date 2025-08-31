using CnC.Application.Abstracts.Repositories.IProductCurrencyRepository;
using CnC.Application.Abstracts.Repositories.IProductDescriptionRepository;
using CnC.Application.Abstracts.Repositories.IProductFilesRepository;
using CnC.Application.Abstracts.Repositories.IProductRepositories;
using CnC.Application.Abstracts.Services;
using CnC.Application.Shared.Responses;
using MediatR;
using System.Net;

namespace CnC.Application.Features.Product.Commands.Delete;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommandRequest, BaseResponse<string>>
{
    private readonly IProductWriteRepository _productWriteRepository;
    private readonly IProductReadRepository _productReadRepository;
    private readonly IProductDescriptionWriteRepository _productDescriptionWriteRepository;
    private readonly IProductDescriptionReadRepository _productDescriptionReadRepository;
    private readonly IProductCurrencyWriteRepository _productCurrencyWriteRepository;
    private readonly IProductFilesWriteRepository _productFilesWriteRepository;
    private readonly IFileServices _fileService;

    public DeleteProductCommandHandler(
        IProductWriteRepository productWriteRepository,
        IProductReadRepository productReadRepository,
        IProductDescriptionWriteRepository productDescriptionWriteRepository,
        IProductDescriptionReadRepository productDescriptionReadRepository,
        IProductCurrencyWriteRepository productCurrencyWriteRepository,
        IProductFilesWriteRepository productFilesWriteRepository,
        IFileServices fileService)
    {
        _productWriteRepository = productWriteRepository;
        _productReadRepository = productReadRepository;
        _productDescriptionWriteRepository = productDescriptionWriteRepository;
        _productDescriptionReadRepository = productDescriptionReadRepository;
        _productCurrencyWriteRepository = productCurrencyWriteRepository;
        _productFilesWriteRepository = productFilesWriteRepository;
        _fileService = fileService;
    }

    public async Task<BaseResponse<string>> Handle(DeleteProductCommandRequest request, CancellationToken cancellationToken)
    {
        var product = await _productReadRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product == null)
            return new("Product not found", HttpStatusCode.NotFound);

        if (!string.IsNullOrEmpty(product.PreviewImageUrl))
            await _fileService.DeleteFileAsync(product.PreviewImageUrl);

        if (product.ProductDescription == null)
            return new("Product description not found", HttpStatusCode.NotFound);

        var description = await _productDescriptionReadRepository.GetByIdWithFilesAsync(product.ProductDescription.Id, cancellationToken);
        if (description == null)
            return new ("Description not found", HttpStatusCode.NotFound);

        if (!string.IsNullOrEmpty(description.ImageUrl))
            await _fileService.DeleteFileAsync(description.ImageUrl);

        foreach (var file in description.ProductFiles.ToList())
        {
            if (!string.IsNullOrEmpty(file.FileUrl))
                await _fileService.DeleteFileAsync(file.FileUrl);
            _productFilesWriteRepository.Delete(file);
        }
        await _productFilesWriteRepository.SaveChangeAsync();

        description.IsDeleted = true;
        description.UpdatedAt = DateTime.UtcNow;
        _productDescriptionWriteRepository.Update(description);
        await _productDescriptionWriteRepository.SaveChangeAsync();

        var currencies = await _productCurrencyWriteRepository.GetByProductIdAsync(request.ProductId);

        foreach (var currency in currencies)
        {
            currency.IsDeleted = true;
            _productCurrencyWriteRepository.Update(currency); 
        }
        await _productCurrencyWriteRepository.SaveChangeAsync();

        product.IsDeleted = true;
        product.UpdatedAt = DateTime.UtcNow;
        _productWriteRepository.Update(product);
        await _productWriteRepository.SaveChangeAsync();

        return new("Product and related files/images deleted successfully", true, HttpStatusCode.OK);
    }
}

