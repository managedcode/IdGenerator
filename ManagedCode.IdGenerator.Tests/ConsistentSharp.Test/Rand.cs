using System;
using System.Security.Cryptography;

namespace ManagedCode.IdGenerator.Tests.ConsistentSharp.Test
{
    internal static class Rand
    {
        // https://stackoverflow.com/questions/6299197/rngcryptoserviceprovider-generate-number-in-a-range-faster-and-retain-distribu
        internal static int Next(int minValue, int maxValue)
        {
            if (minValue > maxValue)
            {
                throw new ArgumentOutOfRangeException("minValue");
            }
            if (minValue == maxValue)
            {
                return minValue;
            }
            var uint32Buffer = new byte[4];
            using (var cr = RandomNumberGenerator.Create())
            {
                long diff = maxValue - minValue;
                while (true)
                {
                    cr.GetBytes(uint32Buffer);
                    var rand = BitConverter.ToUInt32(uint32Buffer, 0);

                    var max = 1 + (long) uint.MaxValue;
                    var remainder = max % diff;
                    if (rand < max - remainder)
                    {
                        return (int) (minValue + rand % diff);
                    }
                }
            }
        }

        internal static int Intn(int maxValue)
        {
            return Next(0, maxValue);
        }
    }
}