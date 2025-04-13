using System.Runtime.InteropServices;
using ElectronicVoting.Admin.Infrastructure.Lirisi.Models;

namespace ElectronicVoting.Admin.Infrastructure.Lirisi;

public static class LirisiHelper
{
    public static GoString ToGoString(string str)
    {
        byte[] strBytes = System.Text.Encoding.UTF8.GetBytes(str);
        IntPtr p = Marshal.AllocHGlobal(strBytes.Length);
        Marshal.Copy(strBytes, 0, p, strBytes.Length);

        return new GoString
        {
            p = p,
            n = (IntPtr)strBytes.Length
        };
    }

    public static GoSlice ToGoSlice(byte[] bytes, out GCHandle handle)
    {
        handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
        return new GoSlice
        {
            data = handle.AddrOfPinnedObject(),
            len = bytes.Length,
            cap = bytes.Length
        };
    }
}