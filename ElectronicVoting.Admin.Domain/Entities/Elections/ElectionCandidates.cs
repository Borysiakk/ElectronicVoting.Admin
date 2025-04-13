namespace ElectronicVoting.Admin.Domain.Entities.Elections;

public class ElectionCandidates :Entity
{
    public long ElectionId { get; set; }
    public Election Election { get; set; }
    public long CandidateId { get; set; }
    public Candidate Candidate { get; set; }
}