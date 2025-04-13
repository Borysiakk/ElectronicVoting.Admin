namespace ElectronicVoting.Admin.Infrastructure.Lirisi.Models;

using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public struct GoSlice
{
    public IntPtr data;
    public long len;
    public long cap;
}