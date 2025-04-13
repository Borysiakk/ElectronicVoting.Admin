using ElectronicVoting.Admin.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElectronicVoting.Admin.Infrastructure.EntityFramework.Configurations;

public class ElectionConfiguration : IEntityTypeConfiguration<Election>
{
    public void Configure(EntityTypeBuilder<Election> builder)
    {
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Name).IsRequired()
            .HasMaxLength(100);
        
        builder.Property(e => e.EndDate).IsRequired();
        builder.Property(e => e.StartDate).IsRequired();

        // Relacja jeden-do-wielu: Election -> ElectionVoters
        builder
            .HasMany(e => e.ElectionVoters)
            .WithOne(ev => ev.Election) 
            .HasForeignKey(ev => ev.ElectionId) 
            .OnDelete(DeleteBehavior.Cascade);
        
        // Relacja jeden-do-wielu: Election -> PaillierKeys
        builder
            .HasMany(e => e.PaillierKeys)
            .WithOne(ev => ev.Election) 
            .HasForeignKey(ev => ev.ElectionId) 
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasData(new Election()
        {
            Id = 1,
            Name = "Election 1",
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1)
        });

    }
}