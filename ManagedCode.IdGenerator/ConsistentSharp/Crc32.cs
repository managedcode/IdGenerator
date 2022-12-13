namespace ManagedCode.IdGenerator.ConsistentSharp
{
    // This code is borrowed from https://github.com/damieng/DamienGKit/blob/master/CSharp/DamienG.Library/Security/Cryptography/Crc32.cs 
    internal static class Crc32
    {
        private const uint DefaultPolynomial = 0xEDB88320;

        private static readonly uint[] Table = Enumerable.Range(0, 256).Select(i =>
        {
            var entry = (uint) i;

            for (var j = 0; j < 8; j++)
            {
                if ((entry & 1) == 1)
                {
                    entry = (entry >> 1) ^ DefaultPolynomial;
                }
                else
                {
                    entry = entry >> 1;
                }
            }

            return entry;
        }).ToArray();

        public static uint Hash(byte[] data)
        {
            return ~data.Aggregate(0xFFFFFFFFU, (hash, b) => (hash >> 8) ^ Table[b ^ (hash & 0xFF)]);
        }
    }
}
