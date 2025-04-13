using System.Runtime.InteropServices;

namespace ElectronicVoting.Admin.Infrastructure.Lirisi.Models;

[StructLayout(LayoutKind.Sequential)]
public struct GoString
{
    public IntPtr p;
    public IntPtr n;
}