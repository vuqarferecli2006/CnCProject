using CnC.Application.Features.Payment.Commands;
using CnC.Domain.Enums;
using FluentAssertions;
using Microsoft.AspNetCore.Http;

namespace _3D_CnC.UnitTest.PaymentTest;

public class CreatePayment_UserNotFoundTests : PaymentTestBase
{
    [Fact]
    public async Task Handle_ShouldReturnUnauthorized_WhenUserNotFound()
    {
        HttpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext());

        var request = new CreatePaymentCommandRequest
        {
            OrderId = Guid.NewGuid(),
            PaymentId = Guid.NewGuid(),
            Currency = Currency.AZN
        };

        var result = await Handler.Handle(request, CancellationToken.None);

        result.Succes.Should().BeFalse();
        result.Message.Should().Be("User not found");
    }
}
