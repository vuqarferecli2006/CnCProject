using CnC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CnC.Percistance.Configurations.ProductBasketConfiguration;

public class ProductBasketConfiguration : IEntityTypeConfiguration<ProductBasket>
{
    public void Configure(EntityTypeBuilder<ProductBasket> builder)
    {
        builder.HasKey(pb => new { pb.ProductId, pb.BasketId });

        builder.HasOne(pb => pb.Product)
               .WithMany(p => p.ProductBaskets)
               .HasForeignKey(pb => pb.ProductId);

        builder.HasOne(pb => pb.Basket)
               .WithMany(b => b.BasketItems)
               .HasForeignKey(pb => pb.BasketId);
    }
}

