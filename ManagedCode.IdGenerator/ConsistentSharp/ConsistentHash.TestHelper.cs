namespace ManagedCode.IdGenerator.ConsistentSharp
{
    public partial class ConsistentHash
    {
        internal Dictionary<uint, string> Circle => _circle;
        internal long Count => _count;
        internal uint[] SortedHashes => _sortedHashes;
    }
}