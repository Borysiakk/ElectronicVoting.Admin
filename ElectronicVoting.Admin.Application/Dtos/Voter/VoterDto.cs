using ElectronicVoting.Admin.Application.Dtos.Election;

namespace ElectronicVoting.Admin.Application.Dtos.Voter;

public class VoterDto :VoterBaseDto
{
    public IEnumerable<ElectionDto> Elections { get; set; }
    public IEnumerable<VoterPublicKeyDto> PublicKeys { get; set; }
}