namespace ElectronicVoting.Admin.Domain.Entities;

public class Entity
{
    public long Id { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
}
