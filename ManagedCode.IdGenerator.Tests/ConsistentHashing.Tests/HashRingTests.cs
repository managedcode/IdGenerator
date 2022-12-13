using ManagedCode.IdGenerator.ConsistentHashing;
using Xunit.Abstractions;

namespace ManagedCode.IdGenerator.Tests.ConsistentHashing.Tests
{
#if false
    public class BstHashRingTests : HashRingTestBase
    {
        protected override IConsistentHashRing<int> CreateRing()
        {
            return new BstHashRing<int>();
        }
    }
#endif

    public class HashRingTests : HashRingTestBase
    {
        public HashRingTests(ITestOutputHelper output) : base(output)
        {
        }

        protected override IConsistentHashRing<int> CreateRing()
        {
            return new HashRing<int>();
        }
    }
}
