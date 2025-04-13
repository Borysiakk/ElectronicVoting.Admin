namespace ElectronicVoting.Admin.Application.Dtos;

public class CandidateDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    
    public int Age { get; set; }
    public string Party { get; set; } // Partia polityczna
    public string Instagram { get; set; } // Link do profilu na Instagramie
    public string Facebook { get; set; } // Link do profilu na Facebooku
    public string Twitter { get; set; } // Link do profilu na Twitterze
    public string Website { get; set; } // Link do strony internetowej
}
