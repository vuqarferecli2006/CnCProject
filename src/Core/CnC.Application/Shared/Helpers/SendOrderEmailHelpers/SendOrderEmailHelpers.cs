using CnC.Application.Abstracts.Services;
using CnC.Application.DTOs.Email;
using CnC.Domain.Entities;

namespace CnC.Application.Shared.Helpers.SendOrderEmailHelpers;

public static class SendOrderEmailHelpers
{
    public static async Task SendOrderEmailsAsync(
        IEmailQueueService emailQueueService,
        Order order,
        string currencyCode,
        decimal totalPrice,
        decimal currencyRate)
    {
        if (order == null) throw new ArgumentNullException(nameof(order));

        var buyerName = order.User?.FullName ?? "Buyer";
        var buyerEmail = order.User?.Email;

        var products = order.OrderProducts
            .Select(op => new
            {
                ProductName = op.Product?.Name ?? "Unknown Product",
                Price = Math.Round(op.UnitPrice / currencyRate, 2),
                Seller = op.Product?.User
            })
            .ToList();

        if (!string.IsNullOrWhiteSpace(buyerEmail))
        {
            var productList = string.Join(Environment.NewLine,
                products.Select(p => $"{p.ProductName} - Price: {p.Price} {currencyCode}"));

            var buyerSubject = $"Payment Confirm - Order #{order.Id}";
            var buyerBody =
                $"Hello {buyerName},\n" +
                $"Payment for order confirmed.\n\n" +
                $"Order Details:\n{productList}\n" +
                $"Total Amount: {totalPrice} {currencyCode}";

            var buyerMessage = new EmailMessageDto
            {
                To = new List<string> { buyerEmail },
                Subject = buyerSubject,
                Body = buyerBody
            };

            await emailQueueService.PublishAsync(buyerMessage);
        }

        var sellersGroups = products
            .Where(p => p.Seller != null && !string.IsNullOrWhiteSpace(p.Seller.Email))
            .GroupBy(p => p.Seller!.Email);

        foreach (var group in sellersGroups)
        {
            var seller = group.First().Seller!;
            var sellerProducts = string.Join(Environment.NewLine,
                group.Select(p => $"{p.ProductName} - Price: {p.Price} {currencyCode}"));

            var totalPriceForSeller = group.Sum(p => p.Price);

            var sellerSubject = $"New Payment - Order #{order.Id}";
            var sellerBody =
                $"Hello {seller.FullName},\n" +
                $"Buyer: {buyerName}\n" +
                $"Payment has been confirmed for your following products:\n{sellerProducts}\n" +
                $"Total Amount: {totalPriceForSeller} {currencyCode}";

            var sellerMessage = new EmailMessageDto
            {
                To = new List<string> { seller.Email },
                Subject = sellerSubject,
                Body = sellerBody
            };

            await emailQueueService.PublishAsync(sellerMessage);
        }
    }
}
