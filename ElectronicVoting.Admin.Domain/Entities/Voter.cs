using ElectronicVoting.Admin.Domain.Entities.Elections;

namespace ElectronicVoting.Admin.Domain.Entities;

public class Voter : Entity
{
    public string Name { get; set; }
    public string Lastname { get; set; }
    public string PersonalIdentity { get; set; }
    public string City { get; set; }
    public string Province { get; set; }
    public DateTime DateOfBirth { get; set; }
    
    public UserCredentials UserCredentials { get; set; }
    public ICollection<VoterPublicKey> PublicKeys { get; set; }
    public ICollection<ElectionVoters> ElectionVoters  { get; set; }
}
