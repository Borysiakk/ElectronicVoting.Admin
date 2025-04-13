namespace ElectronicVoting.Admin.Application.Dtos;

public class RefreshTokenDto
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public DateTime Expires { get; set; }
}