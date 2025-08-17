using CnC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CnC.Percistance.Configurations.ProductConfiguration;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(p => p.PreviewImageUrl)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(p => p.Price)
            .HasPrecision(18, 2);

        builder.Property(p => p.DiscountedPercent)
            .HasPrecision(5, 2);

        builder.Property(p => p.Model)
            .HasMaxLength(100);

        builder.Property(p => p.Curreny)
            .IsRequired();
        
        builder.HasOne(p => p.Category)
               .WithMany(c => c.Products)
               .HasForeignKey(p => p.CategoryId);

        builder.HasOne(p => p.User)
               .WithMany(u => u.Products)
               .HasForeignKey(p => p.UserId);

        builder.HasOne(p => p.ProductDescription)
               .WithOne(pd => pd.Product)
               .HasForeignKey<ProductDescription>(pd => pd.ProductId);

        builder.HasMany(p => p.ProductBaskets)
               .WithOne(pb => pb.Product)
               .HasForeignKey(pb => pb.ProductId);

        builder.HasMany(p => p.ProductViews)
               .WithOne(pv => pv.Product)
               .HasForeignKey(pv => pv.ProductId);

        builder.HasMany(p => p.BookMarks)
               .WithOne(bm => bm.Product)
               .HasForeignKey(bm => bm.ProductId);

        builder.HasMany(p => p.OrderItems)
               .WithOne(op => op.Product)
               .HasForeignKey(op => op.ProductId);
    }
}
