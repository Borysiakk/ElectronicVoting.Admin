namespace ElectronicVoting.Admin.Application.Dtos.Voter;

public class VoterPublicKeyDto
{
    public long Id { get; set; }
    public long ElectionId { get; set; }
    public byte[] PublicKey { get; set; }
    public string PublicKeyBase64 { get; set; }
}