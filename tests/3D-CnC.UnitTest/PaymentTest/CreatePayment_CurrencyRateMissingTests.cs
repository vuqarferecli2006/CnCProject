using CnC.Application.Features.Payment.Commands;
using CnC.Domain.Entities;
using CnC.Domain.Enums;
using FluentAssertions;
using Moq;

namespace _3D_CnC.UnitTest.PaymentTest;

public class CreatePayment_CurrencyRateMissingTests : PaymentTestBase
{
    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenCurrencyRateMissing()
    {
        // Arrange
        var userId = "test-user";
        SetupHttpContext(userId);

        var order = CreateTestOrder(userId);
        OrderReadRepoMock.Setup(x => x.GetOrderWithAllDetailsAsync(order.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);

        var paymentMethod = CreatePaymentMethod(userId);
        PaymentMethodReadRepoMock.Setup(x => x.GetByIdAsync(paymentMethod.Id)).ReturnsAsync(paymentMethod);

        CurrencyRateReadRepoMock.Setup(x => x.GetCurrencyRateByCodeAsync("USD", It.IsAny<CancellationToken>()))
            .ReturnsAsync((CurrencyRate)null);

        var request = new CreatePaymentCommandRequest
        {
            OrderId = order.Id,
            PaymentId = paymentMethod.Id,
            Currency = Currency.USD
        };

        // Act
        var result = await Handler.Handle(request, CancellationToken.None);

        // Assert
        result.Succes.Should().BeFalse();
        result.Message.Should().Be("Currency rate not found");
    }
}
