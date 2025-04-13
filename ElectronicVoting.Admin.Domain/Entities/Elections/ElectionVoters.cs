namespace ElectronicVoting.Admin.Domain.Entities.Elections;

public class ElectionVoters : Entity
{
    public bool IsRegistered { get; set; }
    public long VoterId { get; set; }
    public Voter Voter { get; set; }
    
    public long ElectionId { get; set; }
    public Election Election { get; set; }
}