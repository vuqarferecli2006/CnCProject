using CnC.Application.Abstracts.Repositories.IPaymentMethodRepositories;
using CnC.Application.Shared.Responses;
using CnC.Domain.Entities;
using Google.Apis.Upload;
using MediatR;
using System.Net;

namespace CnC.Application.Features.PaymentMethod;

public class CreatePaymentMethodCommandHandler : IRequestHandler<CreatePaymentMethodCommandRequest, BaseResponse<object>>  
{
    private readonly IPaymentMethodWriteRepository _paymentMethodWriteRepository;

    public CreatePaymentMethodCommandHandler(IPaymentMethodWriteRepository paymentMethodWriteRepository)
    {
        _paymentMethodWriteRepository = paymentMethodWriteRepository;
    }

    public async Task<BaseResponse<object>> Handle(CreatePaymentMethodCommandRequest request, CancellationToken cancellationToken)
    {

        var paymentMethod = new Domain.Entities.PaymentMethod
        {
            Id=Guid.NewGuid(),
            Currency=request.Currency,
            Name=request.Name,
            MethodForPayment=request.MethodForPayment,
        };
        
        await _paymentMethodWriteRepository.AddAsync(paymentMethod);
        await _paymentMethodWriteRepository.SaveChangeAsync();


        return new("Success", paymentMethod, true, HttpStatusCode.OK);
    }
}
