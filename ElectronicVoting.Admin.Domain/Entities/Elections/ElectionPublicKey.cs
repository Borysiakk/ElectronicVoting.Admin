namespace ElectronicVoting.Admin.Domain.Entities.Elections;

public class ElectionPublicKey : Entity
{
    public byte[] PublicKey { get; set; }
    
    public long ElectionId { get; set; }
    public Election Election { get; set; }
}