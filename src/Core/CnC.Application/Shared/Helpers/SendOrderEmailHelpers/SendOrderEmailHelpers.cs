using CnC.Application.Abstracts.Services;
using CnC.Domain.Entities;

namespace CnC.Application.Shared.Helpers.SendOrderEmailHelpers;

public static class SendOrderEmailHelpers
{
    public static async Task SendOrderEmailsAsync(IEmailService emailService, Order order)
    {
        if (order == null) throw new ArgumentNullException(nameof(order));

        var buyerName = order.User?.FullName ?? "Buyer";
        var buyerEmail = order.User?.Email;

        var products = order.OrderProducts
            .Select(op => new
            {
                ProductName = op.Product?.Name ?? "Unknown Product",
                Price = op.UnitPrice,
                Seller = op.Product?.User
            })
            .ToList();

        if (!string.IsNullOrWhiteSpace(buyerEmail))
        {
            var productList = string.Join(Environment.NewLine, products.Select(p => $"{p.ProductName} - Price: {p.Price:C}"));
            var totalPrice = products.Sum(p => p.Price);

            var buyerSubject = $"Payment Confirm - Order #{order.Id}";
            var buyerBody = $"Hello {buyerName},\nPayment for order confirmed.\n\nOrder Details:\n{productList}\nTotal Amount: {totalPrice:C}";

            await emailService.SendEmailAsync(new[] { buyerEmail }, buyerSubject, buyerBody);
        }

        var sellersGroups = products
            .Where(p => p.Seller != null && !string.IsNullOrWhiteSpace(p.Seller.Email))
            .GroupBy(p => p.Seller!.Email);

        foreach (var group in sellersGroups)
        {
            var seller = group.First().Seller!;
            var sellerProducts = string.Join(Environment.NewLine, group.Select(p => $"{p.ProductName} - Price: {p.Price:C}"));
            var totalPriceForSeller = group.Sum(p => p.Price);

            var sellerSubject = $"New Payment - Order #{order.Id}";
            var sellerBody = $"Hello {seller.FullName},\nBuyer: {buyerName}\nPayment has been confirmed for your following products:\n{sellerProducts}\nTotal Amount: {totalPriceForSeller:C}";

            await emailService.SendEmailAsync(new List<string>{ seller.Email }, sellerSubject, sellerBody);
        }
    }

}
