using CnC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CnC.Percistance.Configurations.OrderProductConfiguration;

public class OrderProductConfiguration : IEntityTypeConfiguration<OrderProduct>
{
    public void Configure(EntityTypeBuilder<OrderProduct> builder)
    {
        builder.HasOne(op => op.Order)
               .WithMany(o => o.OrderProducts)
               .HasForeignKey(op => op.OrderId);

        builder.HasOne(op => op.Product)
               .WithMany(p => p.OrderItems)
               .HasForeignKey(op => op.ProductId);

        builder.HasOne(op => op.Download)
               .WithOne(d => d.OrderProduct)
               .HasForeignKey<Download>(d => d.OrderProductId);
    }
}

