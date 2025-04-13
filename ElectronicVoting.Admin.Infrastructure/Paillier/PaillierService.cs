using System.Numerics;
using System.Text;
using ElectronicVoting.Admin.Infrastructure.Paillier.Models;
using FluentResults;

namespace ElectronicVoting.Admin.Infrastructure.Paillier;

public interface IPaillierService
{
    Result<PaillierKeysResult> Generate(int bitLength, int k);
    BigInteger Encryption(string msg, PaillierPublicKeyResult keyPublic);
    BigInteger Decryption(BigInteger encrypted, PaillierPrivateKeyResult keyPrivate);
}

public class PaillierService: IPaillierService
{
    public Result<PaillierKeysResult> Generate(int bitLength, int k)
    {
        BigInteger g;
        BigInteger r = 0;
        var key = new BigInteger[2];
        
        if (bitLength <= 8)
            return Result.Fail("bitLength is too short");
        
        do
        {
            for (var i = 0; i < 2; i++)
            {
                do
                {
                    r = PaillierMath.Random(bitLength);
                } while (!r.IsPrime(k));

                key[i] = r;
            }

        } while (BigInteger.GreatestCommonDivisor(key[0] * key[1], (key[0] - 1) * (key[1] - 1)) != 1);
        
        var lambda = PaillierMath.Lcm(key[0] - 1, key[1] - 1);
        var n = key[0] * key[1];
        
        do
        {
            g = PaillierMath.Random(1, BigInteger.Pow(n, 2) - 1);
        } while (BigInteger.GreatestCommonDivisor(g, n) != 1);

        var mi = PaillierMath.ModularExponentiation(g, lambda, BigInteger.Pow(n, 2));
        var L = (mi - 1) / n;

        mi = PaillierMath.ReciprocalModulo(L, n);
        
        return Result.Ok(new PaillierKeysResult()
        {
            PublicKey = new PaillierPublicKeyResult()
            {
                N = n.ToString(),
                G = g.ToString(),
            },
            PrivateKey = new PaillierPrivateKeyResult()
            {
                Lambda = lambda.ToString(),
                Mi = mi.ToString(),
                P = key[0].ToString(),
                Q = key[1].ToString()
            }
        });
    }

    public BigInteger Encryption(string msg, PaillierPublicKeyResult keyPublic)
    {
        BigInteger r = 1;
        var g = BigInteger.Parse(keyPublic.G);
        var n = BigInteger.Parse(keyPublic.N);
        var iMsg = new BigInteger(Encoding.ASCII.GetBytes(msg));

        var C = BigInteger.Pow(n, 2);

        var t = BigInteger.ModPow(g, iMsg, C);
        var tt = BigInteger.ModPow(r, n, C);

        var ttt =  t * tt;
        return ttt;
    }

    public BigInteger Decryption(BigInteger encrypted, PaillierPrivateKeyResult keyPrivate)
    {
        var mi = BigInteger.Parse(keyPrivate.Mi);
        var lambda = BigInteger.Parse(keyPrivate.Lambda);
        var n = BigInteger.Parse(keyPrivate.P) * BigInteger.Parse(keyPrivate.Q);


        var L = BigInteger.ModPow(encrypted, lambda, BigInteger.Pow(n, 2));

        L = BigInteger.Divide(L - 1, n);

        var result = (L * mi) % n;
    
        return result;
    }
}