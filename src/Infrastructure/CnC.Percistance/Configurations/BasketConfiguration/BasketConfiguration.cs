using CnC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CnC.Percistance.Configurations.BasketConfiguration;

public class BasketConfiguration : IEntityTypeConfiguration<Basket>
{
    public void Configure(EntityTypeBuilder<Basket> builder)
    {
        builder.HasKey(b => b.Id);

        builder.HasOne(b => b.User)
               .WithOne(u => u.Basket)
               .HasForeignKey<Basket>(b => b.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(b => b.UserId).IsUnique(); 
    }
}
