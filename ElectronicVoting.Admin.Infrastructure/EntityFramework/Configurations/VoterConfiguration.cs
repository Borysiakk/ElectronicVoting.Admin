using ElectronicVoting.Admin.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElectronicVoting.Admin.Infrastructure.EntityFramework.Configurations;

public class VoterConfiguration : IEntityTypeConfiguration<Voter>
{
    public void Configure(EntityTypeBuilder<Voter> builder)
    {
        builder.HasKey(a => a.Id);
        
        // Właściwości
        builder.Property(a => a.Name).IsRequired().HasMaxLength(25);
        builder.Property(a => a.Lastname).IsRequired().HasMaxLength(25); 
        builder.Property(a => a.PersonalIdentity).IsRequired().HasMaxLength(15);
        //builder.Property(a => a.DateOfBirth).IsRequired();
        builder.Property(a => a.City).HasMaxLength(50);
        builder.Property(a => a.Province).HasMaxLength(50); 
        
        builder.Property(a => a.CreatedDate).IsRequired();
        
        builder
            .HasOne(v => v.UserCredentials)
            .WithOne(uc => uc.Voter)
            .HasForeignKey<UserCredentials>(uc => uc.VoterId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Relacja jeden-do-wielu: Voter -> PublicKeys
        builder
            .HasMany(a => a.PublicKeys)
            .WithOne(p => p.Voter) 
            .HasForeignKey(p => p.VoterId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Relacja jeden-do-wielu: Voter -> ElectionVoters
        builder
            .HasMany(a => a.ElectionVoters)
            .WithOne(ev => ev.Voter) // Zakładam, że Voter jest nawigacją w ElectionVoters
            .HasForeignKey(ev => ev.VoterId)
            .OnDelete(DeleteBehavior.Cascade); 
        
        
        builder.HasData(
            new Voter
            {
                Id = 1,
                Name = "Jan",
                Lastname = "Kowalski",
                PersonalIdentity = "89011234567",
                DateOfBirth = new DateTime(1989, 01, 12),
                City = "Warszawa",
                Province = "Mazowieckie",
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
            },
            new Voter
            {
                Id = 6,
                Name = "Jan",
                Lastname = "Kowalski",
                PersonalIdentity = "89011234567",
                DateOfBirth = new DateTime(1989, 01, 12),
                City = "Warszawa",
                Province = "Mazowieckie",
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
            },
            new Voter
            {
                Id = 2,
                Name = "Anna",
                Lastname = "Nowak",
                PersonalIdentity = "91122345678",
                DateOfBirth = new DateTime(1991, 12, 23),
                City = "Kraków",
                Province = "Małopolskie",
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
            },
            new Voter
            {
                Id = 3,
                Name = "Piotr",
                Lastname = "Wiśniewski",
                PersonalIdentity = "85031567891",
                DateOfBirth = new DateTime(1985, 03, 15),
                City = "Gdańsk",
                Province = "Pomorskie",
                IsActive = false,
                CreatedDate = DateTime.UtcNow,
            },
            new Voter
            {
                Id = 4,
                Name = "Katarzyna",
                Lastname = "Zielińska",
                PersonalIdentity = "92041789012",
                DateOfBirth = new DateTime(1992, 04, 17),
                City = "Łódź",
                Province = "Łódzkie",
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
            },
            new Voter
            {
                Id = 5,
                Name = "Marek",
                Lastname = "Wójcik",
                PersonalIdentity = "80050612345",
                DateOfBirth = new DateTime(1980, 05, 06),
                City = "Wrocław",
                Province = "Dolnośląskie",
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
            }
        );
    }
}