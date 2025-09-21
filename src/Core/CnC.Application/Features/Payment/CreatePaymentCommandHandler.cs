using CnC.Application.Abstracts.Repositories.IDownloadRepositories;
using CnC.Application.Abstracts.Repositories.IOrderRepositories;
using CnC.Application.Abstracts.Repositories.IPaymentRepositories;
using CnC.Application.Abstracts.Repositories.IProductRepositories;
using CnC.Application.Abstracts.Services;
using CnC.Application.Shared.Helpers.SendOrderEmailHelpers;
using CnC.Application.Shared.Responses;
using CnC.Domain.Entities;
using MediatR;
using System.Net;

namespace CnC.Application.Features.Payment;


//This is a handler written for testing

public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommandRequest, BaseResponse<string>>
{
    private readonly IOrderReadRepository _orderReadRepository;
    private readonly IOrderWriteRepository _orderWriteRepository;
    private readonly IPaymentWriteRepository _paymentWriteRepository;
    private readonly IProductWriteRepository _productWriteRepository;
    private readonly IDownloadWriteRepository _downloadWriteRepository;
    private readonly IElasticProductService _elasticProductService;
    private readonly IEmailService _emailService;

    public CreatePaymentCommandHandler(
        IOrderReadRepository orderReadRepository,
        IOrderWriteRepository orderWriteRepository,
        IPaymentWriteRepository paymentWriteRepository,
        IProductWriteRepository productWriteRepository,
        IDownloadWriteRepository downloadWriteRepository,
        IElasticProductService elasticProductService,
        IEmailService emailService)
    {
        _orderReadRepository = orderReadRepository;
        _orderWriteRepository = orderWriteRepository;
        _paymentWriteRepository = paymentWriteRepository;
        _productWriteRepository = productWriteRepository;
        _downloadWriteRepository = downloadWriteRepository;
        _elasticProductService = elasticProductService;
        _emailService = emailService;
    }

    public async Task<BaseResponse<string>> Handle(CreatePaymentCommandRequest request, CancellationToken cancellationToken)
    {
        var order = await _orderReadRepository.GetOrderWithProductsAsync(request.OrderId, cancellationToken);
        if (order is null)
            return new("Order not found", HttpStatusCode.NotFound);

        if(order.isPaid)
            return new("Order already paid",HttpStatusCode.BadRequest);

        order.isPaid = true;
        _orderWriteRepository.Update(order);

        var payment = new Domain.Entities.Payment
        {
            OrderId = order.Id,
            PaymentMethodId = request.PaymentId
        };
        await _paymentWriteRepository.AddAsync(payment);

        var downloads = new List<Domain.Entities.Download>();
        var productsToUpdate = new List<Domain.Entities.Product>();

        foreach (var orderProduct in order.OrderProducts)
        {
            var product = orderProduct.Product;
            if (product is not null)
            {
                product.Score += 10;
                productsToUpdate.Add(product);

                if (product.ProductDescription?.ProductFiles != null)
                {
                    downloads.AddRange(product.ProductDescription.ProductFiles.Select(file => new Domain.Entities.Download
                    {
                        FileUrl = file.FileUrl,
                        DownloadedAt = DateTime.UtcNow,
                        OrderProductId = orderProduct.Id,
                        ProductFilesId = file.Id
                    }));
                }
            }
        }

        if (productsToUpdate.Any())
        {
            _productWriteRepository.UpdateRange(productsToUpdate);
            foreach (var p in productsToUpdate)
            {
                await _elasticProductService.UpdateProductScoreAsync(p.Id, p.Score);
            }
        }

        if (downloads.Any())
            await _downloadWriteRepository.AddRangeAsync(downloads);

        await _orderWriteRepository.SaveChangeAsync();
        await _productWriteRepository.SaveChangeAsync();
        await _paymentWriteRepository.SaveChangeAsync();
        await _downloadWriteRepository.SaveChangeAsync();


        await SendOrderEmailHelpers.SendOrderEmailsAsync(_emailService,order);
        return new("Order successfully paid", true, HttpStatusCode.OK);
    }
}

