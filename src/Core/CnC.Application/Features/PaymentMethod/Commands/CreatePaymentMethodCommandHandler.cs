using CnC.Application.Abstracts.Repositories.IPaymentMethodRepositories;
using CnC.Application.Shared.Responses;
using CnC.Domain.Entities;
using Google.Apis.Upload;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Security.Claims;

namespace CnC.Application.Features.PaymentMethod;

public class CreatePaymentMethodCommandHandler : IRequestHandler<CreatePaymentMethodCommandRequest, BaseResponse<object>>  
{
    private readonly IPaymentMethodWriteRepository _paymentMethodWriteRepository;
    private readonly IHttpContextAccessor _contextAccessor;

    public CreatePaymentMethodCommandHandler(IPaymentMethodWriteRepository paymentMethodWriteRepository,
                                           IHttpContextAccessor httpContextAccessor)
    {
        _paymentMethodWriteRepository = paymentMethodWriteRepository;
        _contextAccessor = httpContextAccessor;
    }

    public async Task<BaseResponse<object>> Handle(CreatePaymentMethodCommandRequest request, CancellationToken cancellationToken)
    {
        var userId= _contextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var paymentMethod = new Domain.Entities.PaymentMethod
        {
            Id=Guid.NewGuid(),
            Name=request.Name,
            MethodForPayment=request.MethodForPayment,
            UserId=userId,
        };
        
        await _paymentMethodWriteRepository.AddAsync(paymentMethod);
        await _paymentMethodWriteRepository.SaveChangeAsync();


        return new("Success", paymentMethod, true, HttpStatusCode.OK);
    }
}
