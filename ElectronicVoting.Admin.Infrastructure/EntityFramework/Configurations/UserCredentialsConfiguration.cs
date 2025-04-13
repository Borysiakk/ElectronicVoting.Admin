using ElectronicVoting.Admin.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElectronicVoting.Admin.Infrastructure.EntityFramework.Configurations;

public class UserCredentialsConfiguration : IEntityTypeConfiguration<UserCredentials>
{
    public void Configure(EntityTypeBuilder<UserCredentials> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Email).IsRequired().HasMaxLength(50);
        builder.Property(a => a.PasswordHash).IsRequired();
        builder.Property(a => a.PasswordSalt).IsRequired();
        builder.Property(a => a.Role).IsRequired();
        builder.HasIndex(a => a.Email).IsUnique();
        
        // Relacja jeden-do-jeden: UserCredentials -> Voter
        builder
            .HasOne(uc => uc.Voter)
            .WithOne(v => v.UserCredentials)
            .HasForeignKey<UserCredentials>(uc => uc.VoterId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasData(new UserCredentials
        {
            Id = 1,
            Email = "example@example.com",
            PasswordHash = "exampleHash",
            PasswordSalt = "exampleSalt",
            Role = "Admin",
            VoterId = 1,
            CreatedDate = DateTime.Now,
            IsActive = true
        });
    }
}