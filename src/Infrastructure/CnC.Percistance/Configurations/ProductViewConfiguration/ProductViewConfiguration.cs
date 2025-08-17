using CnC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CnC.Percistance.Configurations.ProductViewConfiguration;

public class ProductViewConfiguration : IEntityTypeConfiguration<ProductView>
{
    public void Configure(EntityTypeBuilder<ProductView> builder)
    {
        builder.HasKey(pv => pv.Id);

        builder.HasOne(pv => pv.Product)
               .WithMany(p => p.ProductViews)
               .HasForeignKey(pv => pv.ProductId);

        builder.HasOne(pv => pv.User)
               .WithMany(u => u.ProductViews)
               .HasForeignKey(pv => pv.UserId);
    }
}

