namespace ElectronicVoting.Admin.Domain.Entities;

public class PaillierKeys :Entity
{
    public bool IsActive { get; set; }
    public string PublicKey { get; set; }
    public string PrivateKey { get; set; }
    
    public long ElectionId { get; set; }
    public Election Election { get; set; }
}