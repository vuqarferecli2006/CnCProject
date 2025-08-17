using CnC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CnC.Percistance.Configurations.DownloadConfiguration;

public class DownloadConfiguration : IEntityTypeConfiguration<Download>
{
    public void Configure(EntityTypeBuilder<Download> builder)
    {
        builder.HasKey(d => d.Id);

        builder.HasOne(d => d.OrderProduct)
               .WithOne(op => op.Download)
               .HasForeignKey<Download>(d => d.OrderProductId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
