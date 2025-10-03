using CnC.Domain.Enums;

namespace CnC.Application.Shared.Responses;

public class PaymentMethodResponse
{
    public string MethodId { get; set; }=null!;
    public MethodForPayment Method { get; set; }
}
