using BenchmarkDotNet.Running;

namespace ManagedCode.IdGenerator.Benchmarks;

public class Program
{
    public static void Main(string[] args)
    {
        var summary = BenchmarkRunner.Run<Benchmarker>();
    }
}