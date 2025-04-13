namespace ElectronicVoting.Admin.Domain.Entities;

public class RefreshToken : Entity
{
    public string Token { get; set; }
    public string Email { get; set; } 
    public DateTime Expires { get; set; }
}