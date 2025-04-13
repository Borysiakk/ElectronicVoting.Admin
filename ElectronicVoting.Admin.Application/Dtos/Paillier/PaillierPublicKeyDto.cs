namespace ElectronicVoting.Admin.Application.Dtos.Paillier;

public sealed class PaillierPublicKeyDto
{
    public string N { get; set; }
    public string G { get; set; }
    public string R { get; set; }
}