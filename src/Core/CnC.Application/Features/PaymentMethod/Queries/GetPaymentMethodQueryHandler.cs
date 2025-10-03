using CnC.Application.Abstracts.Repositories.IPaymentMethodRepositories;
using CnC.Application.Shared.Responses;
using CnC.Domain.Entities;
using CnC.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Security.Claims;

namespace CnC.Application.Features.PaymentMethod.Queries;

public class GetPaymentMethodQueryHandler : IRequestHandler<GetPaymentMethodQueryRequest, BaseResponse<List<PaymentMethodResponse>>>
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IPaymentMethodReadRepository _paymentMethodReadRepository;

    public GetPaymentMethodQueryHandler(
        IHttpContextAccessor contextAccessor,
        IPaymentMethodReadRepository paymentMethodReadRepository)
    {
        _contextAccessor = contextAccessor;
        _paymentMethodReadRepository = paymentMethodReadRepository;
    }

    public async Task<BaseResponse<List<PaymentMethodResponse>>> Handle(GetPaymentMethodQueryRequest request,CancellationToken cancellationToken)
    {
        var userId = _contextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return new("Unauthorized", HttpStatusCode.Unauthorized);

        var paymentMethod = await _paymentMethodReadRepository.GetMethodWithUserAsync(userId,cancellationToken);

        if (paymentMethod is null)
            return new("Payment method not found", HttpStatusCode.NotFound);

        var response = paymentMethod.Select(pm => new PaymentMethodResponse
        {
            MethodId = pm.Id.ToString(),
            Method = pm.MethodForPayment
        }).ToList();

        return new("Success", response, true, HttpStatusCode.OK);
    }
}


