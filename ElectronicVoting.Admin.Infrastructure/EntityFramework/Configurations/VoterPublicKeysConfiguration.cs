using ElectronicVoting.Admin.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElectronicVoting.Admin.Infrastructure.EntityFramework.Configurations;

public class VoterPublicKeysConfiguration : IEntityTypeConfiguration<VoterPublicKey>
{
    public void Configure(EntityTypeBuilder<VoterPublicKey> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(pk => pk.IsActive).IsRequired();
        builder.Property(pk => pk.PublicKey).IsRequired().HasMaxLength(2048);

        // Relacja z Voter (jeden-do-wielu)
        builder
            .HasOne(pk => pk.Voter)
            .WithMany(v => v.PublicKeys)
            .HasForeignKey(pk => pk.VoterId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relacja z Election (jeden-do-wielu)
        builder
            .HasOne(pk => pk.Election)
            .WithMany()
            .HasForeignKey(pk => pk.ElectionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasData(new VoterPublicKey()
        {
            Id = 1,
            ElectionId = 1,
            PublicKey = System.Text.Encoding.UTF8.GetBytes("examplePublicKey"),
            PublicKeyBase64 = "examplePublicKeyBase64",
            IsActive = true,
            VoterId = 1,
        });
    }
}