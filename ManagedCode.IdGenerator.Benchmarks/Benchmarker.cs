using BenchmarkDotNet.Attributes;

namespace ManagedCode.IdGenerator.Benchmarks;

[SimpleJob]
[MemoryDiagnoser]
[MemoryRandomization]
[DisassemblyDiagnoser]
[GcServer(true)]
[GcForce]
public class Benchmarker
{
    [Benchmark(Baseline = true, Description = "Next")]
    public NewId.NewId GetNext()
    {
        return NewId.NewId.Next();
    }

    [Benchmark(Description = "Next(batch)", OperationsPerInvoke = 100)]
    public NewId.NewId[] GetNextBatch()
    {
        return NewId.NewId.Next(100);
    }

    [Benchmark(Description = "NextGuid")]
    public Guid GetNextGuid()
    {
        return NewId.NewId.NextGuid();
    }

    [Benchmark(Description = "NextSequentialGuid")]
    public Guid GetNextSequentialGuid()
    {
        return NewId.NewId.NextSequentialGuid();
    }
}