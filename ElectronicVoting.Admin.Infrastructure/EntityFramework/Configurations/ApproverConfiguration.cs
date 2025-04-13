using ElectronicVoting.Admin.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElectronicVoting.Admin.Infrastructure.EntityFramework.Configurations;

public class ApproverConfiguration: IEntityTypeConfiguration<Approver>
{
    public void Configure(EntityTypeBuilder<Approver> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Name).HasMaxLength(50).IsRequired(true);
        builder.Property(a => a.Host).HasMaxLength(50).IsRequired(true);

        builder.HasIndex(a => a.Name).IsUnique();

        builder.HasData(new Approver[]
        {
            new Approver()
            {
                Id = 1,
                Name = "electronicvote.validator.apiA",
                Host = "http://validatorA:80",
            },
            new Approver()
            {
                Id = 2,
                Name = "electronicvote.validator.apiB",
                Host = "http://ValidatorB:80"
            }
        });


    }
}
