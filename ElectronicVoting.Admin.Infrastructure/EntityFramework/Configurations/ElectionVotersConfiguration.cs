using ElectronicVoting.Admin.Domain.Entities.Elections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElectronicVoting.Admin.Infrastructure.EntityFramework.Configurations;

public class ElectionVotersConfiguration : IEntityTypeConfiguration<ElectionVoters>
{
    public void Configure(EntityTypeBuilder<ElectionVoters> builder)
    {
        builder.HasKey(ev => ev.Id);
        builder.Property(ev => ev.IsRegistered).IsRequired().HasDefaultValue(true);
        
        // Relacja z Voter
        builder
            .HasOne(ev => ev.Voter)
            .WithMany(v => v.ElectionVoters)
            .HasForeignKey(ev => ev.VoterId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relacja z Election
        builder
            .HasOne(ev => ev.Election)
            .WithMany(e => e.ElectionVoters)
            .HasForeignKey(ev => ev.ElectionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasData(new ElectionVoters()
        {
            Id = 1,
            VoterId = 1,
            ElectionId = 1,
        });
    }
}