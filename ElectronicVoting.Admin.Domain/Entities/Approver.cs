namespace ElectronicVoting.Admin.Domain.Entities;

public class Approver : Entity
{
    public string Host { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}