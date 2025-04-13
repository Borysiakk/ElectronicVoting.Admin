using ElectronicVoting.Admin.Domain.Entities.Elections;

namespace ElectronicVoting.Admin.Domain.Entities;

public class Candidate : Entity
{
    public Candidate() { }

    public Candidate(string name, string description, int age, string party, string instagram, string facebook, string twitter, string website)
    {
        Age = age;
        Name = name;
        Party = party;
        Website = website;
        Twitter = twitter;
        Facebook = facebook;
        Instagram = instagram;
        Description = description;
    }

    public int Age { get; set; }
    public string Party { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Instagram { get; set; }
    public string Facebook { get; set; }
    public string Twitter { get; set; } 
    public string Website { get; set; } 
    
    public ICollection<ElectionCandidates> ElectionCandidates { get; set; }
}