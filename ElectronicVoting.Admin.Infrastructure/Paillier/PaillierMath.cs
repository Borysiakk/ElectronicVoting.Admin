using System.Numerics;
using System.Text;

namespace ElectronicVoting.Admin.Infrastructure.Paillier;

public static class PaillierMath
{
    //private static Random RandomInstance => Random.Shared;
    private static readonly ThreadLocal<Random> _random = new ThreadLocal<Random>(() => new Random());

    public static bool IsPrime(this BigInteger n, int k)
    {
        if (n.TestFermat(k) == true)
            return n.TestRabinMiller(k) == true;
        else
            return false;
    }

    public static BigInteger Nwd(BigInteger a, BigInteger b)
    {
        while (b != 0)
        {
            var t = b;
            b = a % b;
            a = t;
        }

        return a;
    }

    public static BigInteger Lcm(BigInteger a, BigInteger b)
    {
        return a * b / Nwd(a, b);
    }

    private static List<bool> ToBinaryArray(this BigInteger v)
    {
        var bins = new List<bool>();

        while (v != 0)
        {
            bins.Add(v % 2 != 0);
            v /= 2;
        }

        return bins;
    }

    private static BigInteger MaximumPowerTwo(BigInteger n)
    {
        BigInteger p = (int)(BigInteger.Log(n) / Math.Log(2));
        return Pow(2, p);
    }

    public static BigInteger Pow(BigInteger n, BigInteger p)
    {
        BigInteger val = 1;
        var bins = p.ToBinaryArray();

        foreach (var bin in bins)
        {
            if (bin == true)
                val *= n;
            n *= n;
        }

        return val;
    }

    public static BigInteger ModularExponentiation(BigInteger n, BigInteger p, BigInteger m)
    {
        BigInteger val = 1;
        var x = n % m;

        var bins = p.ToBinaryArray();

        foreach (var bin in bins)
        {
            if (bin == true)
                val = (val * x) % m;
            x = (x * x) % m;
        }

        return val;
    }

    public static BigInteger ReciprocalModulo(BigInteger n, BigInteger m)
    {
        BigInteger w = 1;
        BigInteger x = 0;
        BigInteger u, z, q = 0;
        u = 1;
        w = n;
        z = m;
        while (w != 0)
        {
            if (w < z)
            {
                q = u;
                u = x;
                x = q;
                q = w;
                w = z;
                z = q;
            }

            q = w / z;
            u -= q * x;
            w -= q * z;
        }

        if (z != 1)
            return 0;

        if (x < 0) x += m;
        return x;
    }


    public static bool TestFermat(this BigInteger n, int k)
    {
        for (var i = 0; i < k; i++)
        {
            var a = Random(1, n - 2);
            var r = BigInteger.ModPow(a, n - 1, n);
            if (r != 1) return false;
        }

        return true;
    }

    public static bool TestRabinMiller(this BigInteger n, int k)
    {
        var d = n - 1;
        while (d % 2 == 0) d /= 2;

        for (var i = 0; i < k; i++)
        {
            var a = Random(2, n - 2);
            var x = BigInteger.ModPow(a, d, n);

            if (x == 1 || x == n - 1) return true;

            while (d != n - 1)
            {
                x = BigInteger.ModPow(x, 2, n);
                d *= 2;

                if (x == 1) return false;
                if (x == n - 1) return true;
            }
        }

        return false;
    }

    public static string ToBinaryString(this BigInteger bigint)
    {
        var bytes = bigint.ToByteArray();
        var idx = bytes.Length - 1;

        var base2 = new StringBuilder(bytes.Length * 8);

        var binary = Convert.ToString(bytes[idx], 2);

        if (binary[0] != '0' && bigint.Sign == 1)
            base2.Append('0');

        base2.Append(binary);

        for (idx--; idx >= 0; idx--)
            base2.Append(Convert.ToString(bytes[idx], 2).PadLeft(8, '0'));

        return base2.ToString().Substring(1, base2.ToString().Length - 1);
    }

    public static BigInteger Random(int bitLength)
    {

        if (bitLength < 8 && bitLength != 0)
            return _random.Value.Next(0, 2 ^ bitLength - 1);
        else
        {
            var value = 0;

            var bits = bitLength % 8;
            bitLength = bitLength - bits;
            var bytes = bitLength / 8;

            if (bits != 0)
                for (var i = bits - 1; i >= 0; i--)
                    value |= _random.Value.Next(0, 2) << i;

            if (bytes == 0)
                return value;

            var bs = new byte[bytes + 2];
            _random.Value.NextBytes(bs);

            bs[bytes] = 0;
            bs[bytes + 1] = 0;

            return new BigInteger(bs) + value;

        }
    }

    public static BigInteger Random(BigInteger start, BigInteger end)
    {
        BigInteger value = 0;
        var res = end - start;
        var bitLength = res.ToBinaryString().Length;

        do
        {
            value = Random(bitLength);
        } while (value + start > end);

        return value + start;
    }
}