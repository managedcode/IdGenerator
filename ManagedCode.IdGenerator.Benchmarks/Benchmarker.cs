using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using Unified;

namespace ManagedCode.IdGenerator.Benchmarks;

[SimpleJob(RunStrategy.ColdStart, targetCount: 3)]
[MemoryDiagnoser]
[MemoryRandomization]
[DisassemblyDiagnoser]
[GcServer(true)]
[GcForce]
public class UuidBenchmarks
{

    [Benchmark(Baseline = true)]
    public Guid Guid_Guid()
    {
        return Guid.NewGuid();
    }
    
    [Benchmark]
    public NewId.NewId NewId_NewId()
    {
        return NewId.NewId.Next();
    }
    
    [Benchmark]
    public Guid NewId_Guid()
    {
        return NewId.NewId.NextGuid();
    }
    
    [Benchmark]
    public Guid NewId_NextSequentialGuid()
    {
        return NewId.NewId.NextSequentialGuid();
    }
    
    [Benchmark]
    public UnifiedId UnifiedId_NewId()
    {
        return UnifiedId.NewId();
    }
    
    [Benchmark]
    public string Hashids_Encode()
    {
        return new Hashids.Hashids().Encode(100500);
    }
    
    

    
    //
    // [Benchmark(Description = "Next(batch)", OperationsPerInvoke = 100)]
    // public NewId.NewId[] GetNextBatch()
    // {
    //     return NewId.NewId.Next(100);
    // }
    
}