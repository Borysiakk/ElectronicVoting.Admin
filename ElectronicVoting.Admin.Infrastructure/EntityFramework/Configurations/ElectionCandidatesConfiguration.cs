using ElectronicVoting.Admin.Domain.Entities;
using ElectronicVoting.Admin.Domain.Entities.Elections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElectronicVoting.Admin.Infrastructure.EntityFramework.Configurations;

public class ElectionCandidatesConfiguration : IEntityTypeConfiguration<ElectionCandidates>
{
    public void Configure(EntityTypeBuilder<ElectionCandidates> builder)
    {
        builder.HasKey(ev => ev.Id);
        
        // Relacja z Candidate
        builder
            .HasOne(ev => ev.Candidate)
            .WithMany(v => v.ElectionCandidates)
            .HasForeignKey(ev => ev.CandidateId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relacja z Election
        builder
            .HasOne(ev => ev.Election)
            .WithMany(e => e.ElectionCandidates)
            .HasForeignKey(ev => ev.ElectionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}