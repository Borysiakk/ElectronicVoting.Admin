using System.Collections;
using ElectronicVoting.Admin.Domain.Entities.Elections;

namespace ElectronicVoting.Admin.Domain.Entities;

public class Election : Entity
{
    public string Name { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime StartDate { get; set; }
    
    public ICollection<PaillierKeys> PaillierKeys  { get; set; }
    public ICollection<ElectionVoters> ElectionVoters  { get; set; }
    public ICollection<ElectionPublicKey> ElectionPublicKeys  { get; set; }
    public ICollection<ElectionCandidates> ElectionCandidates  { get; set; }
}