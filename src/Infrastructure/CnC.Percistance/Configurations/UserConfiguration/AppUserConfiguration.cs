using CnC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CnC.Percistance.Configurations.UserConfiguration;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.FullName)
                   .IsRequired()
                   .HasMaxLength(100);

        builder.Property(u => u.ProfilePictureUrl)
            .HasMaxLength(1000);

        // 1-1: AppUser ↔ Basket
        builder.HasOne(u => u.Basket)
               .WithOne(b => b.User)
               .HasForeignKey<Basket>(b => b.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        // 1-n: AppUser ↔ Product
        builder.HasMany(u => u.Products)
               .WithOne(p => p.User)
               .HasForeignKey(p => p.UserId);

        // 1-n: AppUser ↔ ProductViews
        builder.HasMany(u => u.ProductViews)
               .WithOne(pv => pv.User)
               .HasForeignKey(pv => pv.UserId);

        // 1-n: AppUser ↔ Orders
        builder.HasMany(u => u.Orders)
               .WithOne(o => o.User)
               .HasForeignKey(o => o.UserId);

        // 1-n: AppUser ↔ BookMarks
        builder.HasMany(u => u.BookMarks)
               .WithOne(bm => bm.User)
               .HasForeignKey(bm => bm.UserId);

        builder.HasMany(u=>u.PaymentMethods)
               .WithOne(p => p.User)
               .HasForeignKey(p => p.UserId);

    }
}
