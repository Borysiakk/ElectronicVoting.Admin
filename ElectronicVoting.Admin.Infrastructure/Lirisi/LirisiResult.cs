namespace ElectronicVoting.Admin.Infrastructure.Lirisi;

public class LirisiResult
{
    public int Status { get; set; }
    public byte[] Content { get; set; }
    public string ContentBase64 { get; set; }
}