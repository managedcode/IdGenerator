namespace ManagedCode.IdGenerator.Tests.ConsistentSharp.Test
{
    public class Benchmark
    {
        
    }


    //// allocBytes returns the number of bytes allocated by invoking f.
    //        func allocBytes(f func()) uint64 {
    //            var stats runtime.MemStats
    //            runtime.ReadMemStats(&stats)
    //
    //            t := stats.TotalAlloc
    //            f()
    //
    //            runtime.ReadMemStats(&stats)
    //            return stats.TotalAlloc - t
    //        }
    //
    //        func mallocNum(f func()) uint64 {
    //            var stats runtime.MemStats
    //            runtime.ReadMemStats(&stats)
    //
    //            t := stats.Mallocs
    //            f()
    //
    //            runtime.ReadMemStats(&stats)
    //            return stats.Mallocs - t
    //        }
    //
    //        func BenchmarkAllocations(b* testing.B)
    //        {
    //            x:= New()
    //
    //            x.Add("stays")
    //
    //            b.ResetTimer()
    //
    //            allocSize:= allocBytes(func() {
    //                for i := 0; i < b.N; i++ {
    //                    x.Add("Foo")
    //
    //                    x.Remove("Foo")
    //
    //                }
    //            })
    //            b.Logf("%d: Allocated %d bytes (%.2fx)", b.N, allocSize, float64(allocSize) / float64(b.N))
    //        }
    //
    //        func BenchmarkMalloc(b* testing.B)
    //        {
    //            x:= New()
    //
    //            x.Add("stays")
    //
    //            b.ResetTimer()
    //
    //            mallocs:= mallocNum(func() {
    //                for i := 0; i < b.N; i++ {
    //                    x.Add("Foo")
    //
    //                    x.Remove("Foo")
    //
    //                }
    //            })
    //            b.Logf("%d: Mallocd %d times (%.2fx)", b.N, mallocs, float64(mallocs) / float64(b.N))
    //        }
    //
    //        func BenchmarkCycle(b* testing.B)
    //        {
    //            x:= New()
    //
    //            x.Add("nothing")
    //
    //            b.ResetTimer()
    //
    //            for i := 0; i < b.N; i++ {
    //                x.Add("foo" + strconv.Itoa(i))
    //
    //                x.Remove("foo" + strconv.Itoa(i))
    //
    //            }
    //        }
    //
    //        func BenchmarkCycleLarge(b* testing.B)
    //        {
    //            x:= New()
    //
    //            for i := 0; i < 10; i++ {
    //                x.Add("start" + strconv.Itoa(i))
    //
    //            }
    //            b.ResetTimer()
    //
    //            for i := 0; i < b.N; i++ {
    //                x.Add("foo" + strconv.Itoa(i))
    //
    //                x.Remove("foo" + strconv.Itoa(i))
    //
    //            }
    //        }
    //
    //        func BenchmarkGet(b* testing.B)
    //        {
    //            x:= New()
    //
    //            x.Add("nothing")
    //
    //            b.ResetTimer()
    //
    //            for i := 0; i < b.N; i++ {
    //                x.Get("nothing")
    //
    //            }
    //        }
    //
    //        func BenchmarkGetLarge(b* testing.B)
    //        {
    //            x:= New()
    //
    //            for i := 0; i < 10; i++ {
    //                x.Add("start" + strconv.Itoa(i))
    //
    //            }
    //            b.ResetTimer()
    //
    //            for i := 0; i < b.N; i++ {
    //                x.Get("nothing")
    //
    //            }
    //        }
    //
    //        func BenchmarkGetN(b* testing.B)
    //        {
    //            x:= New()
    //
    //            x.Add("nothing")
    //
    //            b.ResetTimer()
    //
    //            for i := 0; i < b.N; i++ {
    //                x.GetN("nothing", 3)
    //
    //            }
    //        }
    //
    //        func BenchmarkGetNLarge(b* testing.B)
    //        {
    //            x:= New()
    //
    //            for i := 0; i < 10; i++ {
    //                x.Add("start" + strconv.Itoa(i))
    //
    //            }
    //            b.ResetTimer()
    //
    //            for i := 0; i < b.N; i++ {
    //                x.GetN("nothing", 3)
    //
    //            }
    //        }
    //
    //        func BenchmarkGetTwo(b* testing.B)
    //        {
    //            x:= New()
    //
    //            x.Add("nothing")
    //
    //            b.ResetTimer()
    //
    //            for i := 0; i < b.N; i++ {
    //                x.GetTwo("nothing")
    //
    //            }
    //        }
    //
    //        func BenchmarkGetTwoLarge(b* testing.B)
    //        {
    //            x:= New()
    //
    //            for i := 0; i < 10; i++ {
    //                x.Add("start" + strconv.Itoa(i))
    //
    //            }
    //            b.ResetTimer()
    //
    //            for i := 0; i < b.N; i++ {
    //                x.GetTwo("nothing")
    //
    //            }
    //        }
    //
}