namespace ElectronicVoting.Admin.Application.Dtos.Paillier;

public sealed class PaillierPrivateKeyDto
{
    public string P { get; set; }
    public string Q { get; set; }
    public string Mi { get; set; }
    public string Lambda { get; set; }
}