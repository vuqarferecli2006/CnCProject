using CnC.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CnC.Percistance.Contexts;

public class AppDbContext:IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Basket> Baskets { get; set; } = null!;

    public DbSet<Bio> Bios { get; set; } = null!;

    public DbSet<BookMark> BookMarks { get; set; } = null!;

    public DbSet<Category> Categories { get; set; } = null!;

    public DbSet<Download> Downloads { get; set; } = null!;

    public DbSet<FreequentlyAskedQuestion> FreequentlyAskedQuestions { get; set; } = null!;

    public DbSet<InformationModel> InformationModels { get; set; } = null!;

    public DbSet<Order> Orders { get; set; } = null!;

    public DbSet<OrderProduct> OrderProducts { get; set; } = null!;

    public DbSet<Payment> Payments { get; set; } = null!;

    public DbSet<ProductBasket> ProductBaskets { get; set; } = null!;

    public DbSet<ProductDescription> ProductDescriptions { get; set; } = null!;

    public DbSet<ProductView> ProductViews { get; set; } = null!;

    public DbSet<PaymentMethod> PaymentMethods { get; set; } = null!;

    public DbSet<Product> Products { get; set; } = null!;

    public DbSet<CurrencyRate> CurrencyRates { get; set; } = null!;

    public DbSet<ProductCurrency> ProductCurrencies { get; set; } = null!;
}
