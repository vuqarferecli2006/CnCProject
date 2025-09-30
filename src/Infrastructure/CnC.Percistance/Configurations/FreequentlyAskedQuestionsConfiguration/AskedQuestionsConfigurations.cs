using CnC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CnC.Percistance.Configurations.FreequentlyAskedQuestionsConfiguration;

public class AskedQuestionsConfigurations : IEntityTypeConfiguration<FreequentlyAskedQuestion>
{
    public void Configure(EntityTypeBuilder<FreequentlyAskedQuestion> builder)
    {
        builder.Property(fq => fq.Description)
           .HasColumnType("text");
    }
}
