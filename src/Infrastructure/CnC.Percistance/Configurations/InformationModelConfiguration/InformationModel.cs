using CnC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CnC.Percistance.Configurations.InformationModelConfiguration;

public class InformationModel : IEntityTypeConfiguration<Domain.Entities.InformationModel>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.InformationModel> builder)
    {
        builder.Property(im => im.VideoUrl)
            .HasMaxLength(250);

        builder.Property(im => im.Description)
            .HasMaxLength(1000);
    }
}
