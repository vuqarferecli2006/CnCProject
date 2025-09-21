using CnC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CnC.Percistance.Configurations.DownloadConfiguration;

public class DownloadConfiguration : IEntityTypeConfiguration<Download>
{
    public void Configure(EntityTypeBuilder<Download> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.FileUrl)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(d => d.Slug)
            .HasMaxLength(100);

        builder.HasIndex(d => d.Slug)
            .IsUnique();
        builder.HasOne(op=> op.OrderProduct)
               .WithMany(d => d.Downloads)
               .HasForeignKey(d => d.OrderProductId);

        builder.HasOne(pf => pf.ProductFiles)
                .WithMany(d => d.Downloads)
                .HasForeignKey(d => d.ProductFilesId);
    }
}
