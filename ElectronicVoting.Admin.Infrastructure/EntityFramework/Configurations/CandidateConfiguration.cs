using ElectronicVoting.Admin.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElectronicVoting.Admin.Infrastructure.EntityFramework.Configurations;

public class CandidateConfiguration : IEntityTypeConfiguration<Candidate>
{
    public void Configure(EntityTypeBuilder<Candidate> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Name).IsRequired();
        builder.Property(a => a.Description).IsRequired(false);


        builder.HasIndex(a => a.Name).IsUnique();

        builder.HasData(new Candidate
        {
            Id = 1,
            Age = 10,
            Name = "Kisiel Porywczy",
            Party = "PP - Partia Psiego Teroru",
            Description =
                "Kisiel Porywczy jest dynamicznym i energicznym liderem, znanym z niespożytej energii i zapału do działań na rzecz swojej społeczności. Jego celem jest wprowadzenie nowych rozwiązań, które poprawią jakość życia wszystkich mieszkańców.",
            Instagram = "https://www.instagram.com/KisielPorywczy",
            Facebook = "https://www.facebook.com/KisielPorywczy",
            Twitter = "https://www.twitter.com/KisielPorywczy",
            Website = "https://www.KisielPorywczy.pl"
        }, new Candidate
        {
            Id = 2,
            Age = 12,
            Name = "Lord Kefir",
            Party = "PPM - Psia Partia Monarchistyczna",
            Description =
                "Lord Kefir jest dostojnym i doświadczonym politykiem, który swoją karierę związał z obroną tradycyjnych wartości. Jest znany z głębokiego zaangażowania w rozwój edukacji oraz dbałości o kulturę i dziedzictwo narodowe.",
            Instagram = "https://www.instagram.com/LordKefir",
            Facebook = "https://www.facebook.com/LordKefir",
            Twitter = "https://www.twitter.com/LordKefir",
            Website = "https://www.LordKefir.pl"
        }, new Candidate
        {
            Id = 3,
            Age = 8,
            Name = "Solo Wojownik",
            Party = "PPS - Partia Psich Spartan",
            Description =
                "Solo Wojownik jest niezłomnym obrońcą praw i wolności obywatelskich. Jego misją jest walka z niesprawiedliwością i korupcją. Jako przedstawiciel Partii Psich Spartan, promuje twardą dyscyplinę i skuteczność w działaniach.",
            Instagram = "https://www.instagram.com/SoloWojownik",
            Facebook = "https://www.facebook.com/SoloWojownik",
            Twitter = "https://www.twitter.com/SoloWojownik",
            Website = "https://www.SoloWojownik.pl"
        });
    }
}