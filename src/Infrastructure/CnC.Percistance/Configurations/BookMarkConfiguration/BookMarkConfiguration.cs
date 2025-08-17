using CnC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CnC.Percistance.Configurations.BookMarkConfiguration;

public class BookMarkConfiguration : IEntityTypeConfiguration<BookMark>
{
    public void Configure(EntityTypeBuilder<BookMark> builder)
    {
        builder.HasKey(bm => new { bm.ProductId, bm.UserId });

        builder.HasOne(bm => bm.Product)
               .WithMany(p => p.BookMarks)
               .HasForeignKey(bm => bm.ProductId);

        builder.HasOne(bm => bm.User)
               .WithMany(u => u.BookMarks)
               .HasForeignKey(bm => bm.UserId);
    }
}
