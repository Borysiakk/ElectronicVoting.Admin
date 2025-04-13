namespace ElectronicVoting.Admin.Domain.Entities;

public class UserCredentials : Entity
{
    public string Role { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
    
    public long VoterId { get; set; }
    public Voter Voter { get; set; }
}