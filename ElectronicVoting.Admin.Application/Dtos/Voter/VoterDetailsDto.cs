using ElectronicVoting.Admin.Application.Dtos.Election;

namespace ElectronicVoting.Admin.Application.Dtos.Voter;

public class VoterDetailsDto : VoterBaseDto
{
    public string Role { get; set; }
    public string Email { get; set; }
    public IEnumerable<ElectionDetailsDto> Elections { get; set; }
    public IEnumerable<VoterPublicKeyDto> PublicKeys { get; set; }
}