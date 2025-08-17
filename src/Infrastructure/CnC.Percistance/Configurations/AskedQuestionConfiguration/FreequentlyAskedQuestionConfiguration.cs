using CnC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CnC.Percistance.Configurations.AskedQuestionConfiguration;

public class FreequentlyAskedQuestionConfiguration : IEntityTypeConfiguration<FreequentlyAskedQuestion>
{
    public void Configure(EntityTypeBuilder<FreequentlyAskedQuestion> builder)
    {
        builder.Property(f => f.VideoUrl)
            .HasMaxLength(250);

        builder.Property(f => f.Description)
            .HasMaxLength(1000);

        builder.Property(f => f.Question)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(f => f.Answer)
            .IsRequired()
            .HasMaxLength(1000);
    }
}
