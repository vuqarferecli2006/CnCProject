using CnC.Application.Abstracts.Services;
using CnC.Application.Features.Payment.Commands;
using CnC.Domain.Entities;
using CnC.Domain.Enums;
using FluentAssertions;
using Moq;

namespace _3D_CnC.UnitTest.PaymentTest;

public class CreatePayment_SuccessTests : PaymentTestBase
{
    [Fact]
    public async Task Handle_ShouldCreatePaymentAndMarkOrderAsPaid()
    {
        var userId = "test-user";
        SetupHttpContext(userId);

        var order = CreateTestOrder(userId);
        OrderReadRepoMock.Setup(x => x.GetOrderWithAllDetailsAsync(order.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);

        var paymentMethod = CreatePaymentMethod(userId);
        PaymentMethodReadRepoMock.Setup(x => x.GetByIdAsync(paymentMethod.Id)).ReturnsAsync(paymentMethod);

        var strategyMock = new Mock<IPaymentStrategy>();
        strategyMock.Setup(s => s.CreatePaymentIntentAsync(It.IsAny<decimal>(), It.IsAny<string>()))
            .ReturnsAsync("test_pi_123");
        strategyMock.Setup(s => s.ConfirmPaymentAsync("test_pi_123")).ReturnsAsync("succeeded");

        PaymentStrategyFactoryMock.Setup(f => f.GetPaymentStrategy(MethodForPayment.CreditCard)).Returns(strategyMock.Object);

        PaymentWriteRepoMock.Setup(x => x.AddAsync(It.IsAny<Payment>())).Returns(Task.CompletedTask);
        PaymentWriteRepoMock.Setup(x => x.SaveChangeAsync()).Returns(Task.CompletedTask);
        OrderWriteRepoMock.Setup(x => x.Update(It.IsAny<Order>()));
        OrderWriteRepoMock.Setup(x => x.SaveChangeAsync()).Returns(Task.CompletedTask);

        var request = new CreatePaymentCommandRequest
        {
            OrderId = order.Id,
            PaymentId = paymentMethod.Id,
            Currency = Currency.AZN
        };

        var result = await Handler.Handle(request, CancellationToken.None);

        result.Succes.Should().BeTrue();
        result.Message.Should().Be("Payment initiated");
        result.Data.Should().Be("test_pi_123");
    }
}
