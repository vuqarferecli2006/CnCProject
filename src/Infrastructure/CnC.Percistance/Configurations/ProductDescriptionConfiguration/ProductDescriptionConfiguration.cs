using CnC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CnC.Percistance.Configurations.ProductDescriptionConfiguration;

public class ProductDescriptionConfiguration : IEntityTypeConfiguration<ProductDescription>
{
    public void Configure(EntityTypeBuilder<ProductDescription> builder)
    {
        builder.HasKey(pd => pd.Id);

        builder.Property(d => d.ImageUrl)
            .HasMaxLength(250);

        builder.Property(p => p.Description)
            .HasColumnType("text");

        builder.Property(d => d.DiscountedPercent)
            .HasPrecision(5, 2);

        builder.Property(d => d.Model)
            .HasMaxLength(100);

        builder.HasOne(pd => pd.Product)
               .WithOne(p => p.ProductDescription)
               .HasForeignKey<ProductDescription>(pd => pd.ProductId);

        builder.HasMany(pd => pd.ProductFiles)
                .WithOne(pf => pf.ProductDescription)
                .HasForeignKey(pf => pf.ProductDescriptionId);
    }
}

