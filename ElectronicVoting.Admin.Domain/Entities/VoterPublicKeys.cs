namespace ElectronicVoting.Admin.Domain.Entities;

public class VoterPublicKey :Entity
{
    public long VoterId { get; set; }
    public long ElectionId { get; set; }
    
    public byte[] PublicKey { get; set; }
    public string PublicKeyBase64 { get; set; }
    
    public Voter Voter { get; set; }
    public Election Election { get; set; }
    
    
}