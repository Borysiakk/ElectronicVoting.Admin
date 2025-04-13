namespace ElectronicVoting.Admin.Application.Dtos.Election;

public class ElectionDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime StartDate { get; set; }
}