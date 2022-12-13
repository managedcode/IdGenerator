namespace ManagedCode.IdGenerator.ConsistentSharp
{
    public interface IHashAlgorithm
    {
        uint HashKey(string key);
    }
}