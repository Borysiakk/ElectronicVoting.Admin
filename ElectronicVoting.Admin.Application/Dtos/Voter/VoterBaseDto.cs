namespace ElectronicVoting.Admin.Application.Dtos.Voter;

public class VoterBaseDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Lastname { get; set; }
    public string PersonalIdentity { get; set; }
    public bool Status { get; set; }
}