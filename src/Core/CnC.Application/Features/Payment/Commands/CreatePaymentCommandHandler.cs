using CnC.Application.Abstracts.Repositories.ICurrencyRateRepository;
using CnC.Application.Abstracts.Repositories.IDownloadRepositories;
using CnC.Application.Abstracts.Repositories.IOrderRepositories;
using CnC.Application.Abstracts.Repositories.IPaymentMethodRepositories;
using CnC.Application.Abstracts.Repositories.IPaymentRepositories;
using CnC.Application.Abstracts.Repositories.IProductRepositories;
using CnC.Application.Abstracts.Services;
using CnC.Application.Shared.Helpers.SendOrderEmailHelpers;
using CnC.Application.Shared.Responses;
using CnC.Domain.Entities;
using CnC.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Security.Claims;

namespace CnC.Application.Features.Payment.Commands;


//This is a handler written for testing

public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommandRequest, BaseResponse<string>>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IOrderReadRepository _orderReadRepository;
    private readonly ICurrencyRateReadRepository _currencyRateReadRepository;
    private readonly IPaymentMethodReadRepository _paymentMethodReadRepository;
    private readonly IPaymentWriteRepository _paymentWriteRepository;
    private readonly IPaymentStrategyFactory _paymentStrategyFactory;
    private readonly IOrderWriteRepository _orderWriteRepository;
    private readonly IProductWriteRepository _productWriteRepository;
    private readonly IElasticProductService _elasticsearchProductService;
    private readonly IDownloadWriteRepository _downloadWriteRepository;
    private readonly IEmailQueueService _emailQueueService;

    public CreatePaymentCommandHandler(
        IHttpContextAccessor httpContextAccessor,
        IOrderReadRepository orderReadRepository,
        ICurrencyRateReadRepository currencyRateReadRepository,
        IPaymentMethodReadRepository paymentMethodReadRepository,
        IPaymentWriteRepository paymentWriteRepository,
        IPaymentStrategyFactory paymentStrategyFactory,
        IOrderWriteRepository orderWriteRepository,
        IProductWriteRepository productWriteRepository,
        IElasticProductService elasticsearchProductService,
        IDownloadWriteRepository downloadWriteRepository,
        IEmailQueueService emailQueueService)
    {
        _httpContextAccessor = httpContextAccessor;
        _orderReadRepository = orderReadRepository;
        _currencyRateReadRepository = currencyRateReadRepository;
        _paymentMethodReadRepository = paymentMethodReadRepository;
        _paymentWriteRepository = paymentWriteRepository;
        _paymentStrategyFactory = paymentStrategyFactory;
        _orderWriteRepository = orderWriteRepository;
        _productWriteRepository = productWriteRepository;
        _elasticsearchProductService = elasticsearchProductService;
        _downloadWriteRepository = downloadWriteRepository;
        _emailQueueService = emailQueueService;
    }

    public async Task<BaseResponse<string>> Handle(CreatePaymentCommandRequest request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return new("User not found", HttpStatusCode.Unauthorized);

        var order = await _orderReadRepository.GetOrderWithAllDetailsAsync(request.OrderId, cancellationToken);
        if (order is null || order.UserId != userId)
            return new("Order not found or not owned by user", HttpStatusCode.NotFound);

        if (order.isPaid)
            return new("Order already paid", HttpStatusCode.BadRequest);

        if (!order.OrderProducts.Any())
            return new("Order has no products", HttpStatusCode.BadRequest);

        var paymentMethod = await _paymentMethodReadRepository.GetByIdAsync(request.PaymentId);
        if (paymentMethod is null || paymentMethod.UserId != userId)
            return new("Payment method not found", HttpStatusCode.NotFound);

        decimal totalPrice = order.TotalAmount;
        decimal currencyRate = 1;
        if (request.Currency != Currency.AZN)
        {
            var currencyRateEntity = await _currencyRateReadRepository.GetCurrencyRateByCodeAsync(
                request.Currency.ToString(), cancellationToken);

            if (currencyRateEntity is null)
                return new("Currency rate not found", HttpStatusCode.NotFound);

            currencyRate = currencyRateEntity.RateAgainstAzn;
            totalPrice = Math.Round(totalPrice / currencyRate, 2);
        }

        var strategy = _paymentStrategyFactory.GetPaymentStrategy(paymentMethod.MethodForPayment);

        var paymentIntentId = await strategy.CreatePaymentIntentAsync(totalPrice, request.Currency.ToString());
        var status = await strategy.ConfirmPaymentAsync(paymentIntentId);

        order.isPaid = true;
        _orderWriteRepository.Update(order);

        var payment = new Domain.Entities.Payment
        {
            Id = Guid.NewGuid(),
            OrderId = order.Id,
            PaymentMethodId = paymentMethod.Id,
            Currency = request.Currency,
            Amount = totalPrice,
            PaymentIntentId = paymentIntentId,
            CreatedAt = DateTime.UtcNow,
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
            _productWriteRepository.UpdateRange(productsToUpdate);

        if (downloads.Any())
            await _downloadWriteRepository.AddRangeAsync(downloads);

        await _orderWriteRepository.SaveChangeAsync();
        await _productWriteRepository.SaveChangeAsync();
        await _downloadWriteRepository.SaveChangeAsync();
        await _paymentWriteRepository.SaveChangeAsync();

        if (productsToUpdate.Any())
        {
            var updateTasks = productsToUpdate.Select(p =>
                _elasticsearchProductService.UpdateProductScoreAsync(p.Id, p.Score));
            await Task.WhenAll(updateTasks);
        }
        await SendOrderEmailHelpers.SendOrderEmailsAsync(_emailQueueService, order, request.Currency.ToString(), totalPrice, currencyRate);
        return new ("Payment initiated", paymentIntentId, true, HttpStatusCode.OK);
    }

}


