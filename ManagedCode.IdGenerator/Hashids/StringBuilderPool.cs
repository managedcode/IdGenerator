using System.Collections.Concurrent;
using System.Text;

namespace ManagedCode.IdGenerator.Hashids;

internal class StringBuilderPool
{
    private readonly ConcurrentBag<StringBuilder> _builders = new();

    public StringBuilder Get()
    {
        return _builders.TryTake(out var sb) ? sb : new StringBuilder();
    }

    public void Return(StringBuilder sb)
    {
        sb.Clear();
        _builders.Add(sb);
    }
}