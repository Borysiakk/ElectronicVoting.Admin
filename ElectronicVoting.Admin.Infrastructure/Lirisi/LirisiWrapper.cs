using ElectronicVoting.Admin.Infrastructure.Lirisi.Models;

namespace ElectronicVoting.Admin.Infrastructure.Lirisi;

using System.Runtime.InteropServices;

public sealed class LirisiWrapper
{
    private const string LibraryName = "libs/lirisilib.dll";
    
    [DllImport(LibraryName, EntryPoint = "GeneratePrivateKey", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr GeneratePrivateKeyFromDll(GoString curveName, GoString format);
    
    [DllImport(LibraryName, EntryPoint = "DerivePublicKey", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr DerivePublicKeyFromDLL(GoSlice privateKey, GoString format);
    
    [DllImport(LibraryName, EntryPoint = "FoldPublicKeys", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr FoldPublicKeysFromDll(GoSlice pubKeysContent, GoString hashName, GoString outFormat, GoString order);
    
    [DllImport(LibraryName, EntryPoint = "CreateSignature", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr CreateSignatureFromDll(GoSlice foldedPublicKeys, GoSlice privateKeyContent, GoSlice message, GoSlice caseIdentifier, GoString outFormat);
    
    [DllImport(LibraryName, EntryPoint = "VerifySignature", CallingConvention = CallingConvention.Cdecl)]
    private static extern long VerifySignatureFromDLL(GoSlice foldedPublicKeys, GoSlice signature, GoSlice message, GoSlice caseIdentifier);
    
    [DllImport(LibraryName, EntryPoint = "SignatureKeyImage", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr SignatureKeyImageFromDLL(GoSlice signature, bool separator);
    
    [DllImport("libc.so.6", EntryPoint = "free", CallingConvention = CallingConvention.Cdecl)]
    private static extern void FreeLinux(IntPtr ptr);
    
    [DllImport("msvcrt.dll", EntryPoint = "free", CallingConvention = CallingConvention.Cdecl)]
    private static extern void FreeWindows(IntPtr ptr);

    public LirisiResult GeneratePrivateKey(string curveName, string format)
    {
        GoString goFormat = LirisiHelper.ToGoString(format);
        GoString goCurveName = LirisiHelper.ToGoString(curveName);
        
        var resultPtr = GeneratePrivateKeyFromDll(goCurveName, goFormat);
        var result = PointerToLirisiResult(resultPtr);
        
        Marshal.FreeHGlobal(goFormat.p);
        Marshal.FreeHGlobal(goCurveName.p);
        FreeMemory(resultPtr);

        return result;
    }

    public LirisiResult DerivePublicKey(byte[] privateKey, string format)
    {
        var goFormat = LirisiHelper.ToGoString(format);
        var goPrivateKey = LirisiHelper.ToGoSlice(privateKey, out var handle);
        
        var resultPtr = DerivePublicKeyFromDLL(goPrivateKey, goFormat);
        var result = PointerToLirisiResult(resultPtr);
        
        Marshal.FreeHGlobal(goFormat.p);
        FreeMemory(resultPtr);
        handle.Free();
        
        return result;
    }

    public LirisiResult FoldPublicKeys(byte[][] publicKeys, string hashName, string outFormat, string order)
    {
        var goOrder = LirisiHelper.ToGoString(order);
        var goHashName = LirisiHelper.ToGoString(hashName);
        var goOutFormat = LirisiHelper.ToGoString(outFormat);
        
        var handles = new GCHandle[publicKeys.Length];
        var goPublicKeys = new GoSlice[publicKeys.Length];
        var goPublicKeysArray = new GoSlice[publicKeys.Length];
        for (var i = 0; i < publicKeys.Length; i++)
        {
            goPublicKeys[i] = LirisiHelper.ToGoSlice(publicKeys[i], out handles[i]);
            goPublicKeysArray[i] = goPublicKeys[i];
        }
        
        var handleGoPublicKeys = GCHandle.Alloc(goPublicKeysArray, GCHandleType.Pinned);
        var mainSlice = new GoSlice
        {
          data = handleGoPublicKeys.AddrOfPinnedObject(),
          len = goPublicKeysArray.Length,
          cap = goPublicKeysArray.Length
        };
        
        var resultPtr = FoldPublicKeysFromDll(mainSlice, goHashName, goOutFormat, goOrder);
        var result = PointerToLirisiResult(resultPtr);
        
        handleGoPublicKeys.Free();
        foreach (var handle in handles)
            handle.Free();
        
        Marshal.FreeHGlobal(goOrder.p);
        Marshal.FreeHGlobal(goHashName.p);
        Marshal.FreeHGlobal(goOutFormat.p);
        FreeMemory(resultPtr);
        
        return result;
    }

    public LirisiResult CreateSignature(byte[] foldedPublicKeys, byte[] privateKey, byte[] message, byte[] caseIdentifier, string outFormat)
    {
        var goOutFormat = LirisiHelper.ToGoString(outFormat);
        
        var goMessage = LirisiHelper.ToGoSlice(message, out var handleMessage);
        var goPrivateKey = LirisiHelper.ToGoSlice(privateKey, out var handlePrivateKey);
        var goPublicKeys = LirisiHelper.ToGoSlice(foldedPublicKeys, out var handlePublicKeys);
        var goCaseIdentifier = LirisiHelper.ToGoSlice(caseIdentifier, out var handleCaseIdentifier);
        
        var resultPtr = CreateSignatureFromDll(goPublicKeys, goPrivateKey, goMessage, goCaseIdentifier, goOutFormat);
        var result = PointerToLirisiResult(resultPtr);
        
        handleMessage.Free();
        handlePrivateKey.Free();
        handlePublicKeys.Free();
        handleCaseIdentifier.Free();
        Marshal.FreeHGlobal(goOutFormat.p);
        FreeMemory(resultPtr);
        
        return result;
    }
    public long VerifySignature(byte[] foldedPublicKeys, byte[] signature, byte[] message, byte[] caseIdentifier)
    {
        var goMessage = LirisiHelper.ToGoSlice(message, out var handleMessage);
        var goSignature = LirisiHelper.ToGoSlice(signature, out var handleSignature);
        var goPublicKeys = LirisiHelper.ToGoSlice(foldedPublicKeys, out var handlePublicKeys);
        var goCaseIdentifier = LirisiHelper.ToGoSlice(caseIdentifier, out var handleCaseIdentifier);
        
        var result = VerifySignatureFromDLL(goPublicKeys, goSignature, goMessage, goCaseIdentifier);
        
        handleMessage.Free();
        handleSignature.Free();
        handlePublicKeys.Free();
        handleCaseIdentifier.Free();
        
        return result;
    }

    public LirisiResult SignatureKeyImage(byte[] signature, bool separator)
    {
        var goSignature = LirisiHelper.ToGoSlice(signature, out var handleSignature);
        var resultPtr = SignatureKeyImageFromDLL(goSignature, separator);
        var result = PointerToLirisiResult(resultPtr);
        handleSignature.Free();
        FreeMemory(resultPtr);
        
        return result;
    }
    
    private LirisiResult PointerToLirisiResult(IntPtr ptr)
    {
        const int headerSize = 16;
        const int statusOffset = 0;
        const int lengthOffset = 8;

        byte[] headerData = CopyFromPointer(ptr, headerSize);
    
        int status = BitConverter.ToInt32(headerData, statusOffset);
        int contentLength = BitConverter.ToInt32(headerData, lengthOffset);

        byte[] contentData = CopyFromPointer(IntPtr.Add(ptr, headerSize), contentLength);

        return new LirisiResult
        {
            Status = status,
            Content = contentData,
            ContentBase64 = Convert.ToBase64String(contentData)
        };
    }
    
    private static byte[] CopyFromPointer(IntPtr source, int length)
    {
        byte[] buffer = new byte[length];
        Marshal.Copy(source, buffer, 0, length);
        return buffer;
    }
    
    private static void FreeMemory(IntPtr ptr)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            FreeWindows(ptr);
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            FreeLinux(ptr);
        else
            throw new PlatformNotSupportedException("This platform is not supported.");
    }
}