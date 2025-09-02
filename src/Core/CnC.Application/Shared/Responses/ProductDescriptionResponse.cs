using System.Security.Cryptography.X509Certificates;

namespace CnC.Application.Shared.Responses;

public class ProductDescriptionResponse
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = null!;//product table-daki name
    public string Description { get; set; } = null!;//PrdoductDescription table-daki description
    public string Model { get; set; } = null!;//PrdoductDescription table-daki model
    public string ImageUrl { get; set; } = null!;//PrdoductDescription table-daki imageurl  
    public int ViewCount { get; set; }//PrdoductDescription table-daki viewcount
    public decimal DiscountedPercent { get; set; }//product table-daki discountedpercent
    public int Score { get; set; }//product table-daki score
    public decimal Price { get; set; }//product table-daki price
}
