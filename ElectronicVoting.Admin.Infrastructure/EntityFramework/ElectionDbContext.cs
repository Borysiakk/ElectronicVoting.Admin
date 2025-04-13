using ElectronicVoting.Admin.Domain.Entities;
using ElectronicVoting.Admin.Domain.Entities.Elections;
using ElectronicVoting.Admin.Infrastructure.EntityFramework.Configurations;
using Microsoft.EntityFrameworkCore;

namespace ElectronicVoting.Admin.Infrastructure.EntityFramework;

public class ElectionDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Voter> Voters { get; set; }
    public DbSet<Approver> Approvers { get; set; }
    public DbSet<Election> Elections { get; set; }
    public DbSet<Candidate> Candidates { get; set; }
    public DbSet<PaillierKeys> PaillierKeys { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<VoterPublicKey> VoterPublicKeys { get; set; }
    public DbSet<ElectionVoters> ElectionVoters { get; set; }
    public DbSet<UserCredentials> UserCredentials { get; set; }
    public DbSet<ElectionPublicKey> ElectionPublicKey { get; set; }
    public DbSet<ElectionCandidates> ElectionCandidates { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new VoterConfiguration());
        modelBuilder.ApplyConfiguration(new ElectionConfiguration());
        modelBuilder.ApplyConfiguration(new ApproverConfiguration());
        modelBuilder.ApplyConfiguration(new CandidateConfiguration());
        modelBuilder.ApplyConfiguration(new PaillierKeysConfiguration());
        modelBuilder.ApplyConfiguration(new ElectionVotersConfiguration());
        modelBuilder.ApplyConfiguration(new UserCredentialsConfiguration());
        modelBuilder.ApplyConfiguration(new VoterPublicKeysConfiguration());
        modelBuilder.ApplyConfiguration(new ElectionCandidatesConfiguration());
    }
}