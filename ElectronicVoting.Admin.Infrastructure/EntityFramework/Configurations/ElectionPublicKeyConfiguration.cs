using ElectronicVoting.Admin.Domain.Entities.Elections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElectronicVoting.Admin.Infrastructure.EntityFramework.Configurations;

public class ElectionPublicKeyConfiguration : IEntityTypeConfiguration<ElectionPublicKey>
{
    public void Configure(EntityTypeBuilder<ElectionPublicKey> builder)
    {
        builder.HasKey(ev => ev.Id);
        builder.Property(ev => ev.PublicKey).IsRequired().HasMaxLength(2048);
        
        builder
            .HasOne(ev => ev.Election)
            .WithMany(e => e.ElectionPublicKeys)
            .HasForeignKey(ev => ev.ElectionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}