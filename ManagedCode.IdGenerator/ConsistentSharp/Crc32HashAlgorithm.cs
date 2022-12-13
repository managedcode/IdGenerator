using System.Text;

namespace ManagedCode.IdGenerator.ConsistentSharp
{
    public class Crc32HashAlgorithm : IHashAlgorithm
    {
        public uint HashKey(string key) {
            return Crc32.Hash(Encoding.UTF8.GetBytes(key));
        }
    }
}