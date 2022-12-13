using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using ManagedCode.IdGenerator.ConsistentHashing;

namespace ManagedCode.IdGenerator.Benchmarks;

[SimpleJob(RunStrategy.ColdStart, targetCount: 3)]
[MemoryDiagnoser]
[MemoryRandomization]
[DisassemblyDiagnoser]
[GcServer(true)]
[GcForce]
public class HashBenchmarks
{
    private string[] _servers = Enumerable.Range(1, 50).Select(s => "server" + s).ToArray();

    [Benchmark]
    public void ConsistentHashing()
    {
        var hash = new ConsistentHash.ConsistentHash<string>();
        hash.Init(_servers);

        for (int i = 0; i < 10_000; i++)
        {
            var node = hash.GetNode(i.ToString());
        }
    }
    
    [Benchmark]
    public void HashRing()
    {
        var hash = new HashRing<string>();
        uint number = 1;
        foreach (var item in _servers)
        {
            hash.AddNode(item, number);
            number++;
        }
        

        for (int i = 0; i < 10_000; i++)
        {
            var node = hash.GetNode(Convert.ToUInt32(i));
        }
    }
    
    [Benchmark]
    public void ConsistentSharp()
    {
        var hash = new ManagedCode.IdGenerator.ConsistentSharp.ConsistentHash();
        uint number = 1;
        foreach (var item in _servers)
        {
            hash.Add(item);
            number++;
        }
        
        for (int i = 0; i < 10_000; i++)
        {
            var node = hash.Get(i.ToString());
        }
    }

   
}