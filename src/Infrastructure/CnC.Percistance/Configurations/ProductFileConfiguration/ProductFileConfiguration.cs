using CnC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CnC.Percistance.Configurations.ProductFileConfiguration;

public class ProductFileConfiguration : IEntityTypeConfiguration<ProductFiles>
{
    public void Configure(EntityTypeBuilder<ProductFiles> builder)
    {
        builder.HasKey(pf => pf.Id);

        builder.Property(pf => pf.FileUrl)
            .IsRequired()
            .HasMaxLength(1000);

        builder.HasOne(pf => pf.ProductDescription)
               .WithMany(pd => pd.ProductFiles)
               .HasForeignKey(pf => pf.ProductDescriptionId);

        builder.HasMany(pf => pf.Downloads)
                .WithOne(d => d.ProductFiles)
                .HasForeignKey(d => d.ProductFilesId);
    }
}
