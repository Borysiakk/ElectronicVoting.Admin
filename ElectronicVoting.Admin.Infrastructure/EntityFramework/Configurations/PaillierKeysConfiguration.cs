using ElectronicVoting.Admin.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElectronicVoting.Admin.Infrastructure.EntityFramework.Configurations;

public class PaillierKeysConfiguration: IEntityTypeConfiguration<PaillierKeys>
{
    public void Configure(EntityTypeBuilder<PaillierKeys> builder)
    {
        builder.HasKey(p => p.Id);
        builder.HasIndex(p => p.IsActive);
        
        builder.Property(p => p.IsActive)
            .IsRequired();

        builder.Property(p => p.PublicKey)
            .IsRequired()
            .HasMaxLength(2048);

        builder.Property(p => p.PrivateKey)
            .IsRequired()
            .HasMaxLength(2048);
        
        // Relacja z Election
        builder
            .HasOne(ev => ev.Election)
            .WithMany(e => e.PaillierKeys)
            .HasForeignKey(ev => ev.ElectionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}