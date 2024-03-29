﻿using System.Linq;
using FluentAssertions;
using Xunit;

namespace ManagedCode.IdGenerator.Tests.ConsistentHashing.Tests
{
    public class WrappingRangeTest
    {
        [Fact]
        public void TestWrappingRange()
        {
            var range = new WrappingRange(uint.MaxValue - 2, 2).ToList();
            range.Should().Equal(uint.MaxValue - 1, uint.MaxValue, 0, 1, 2);

            range = new WrappingRange(uint.MaxValue, 2).ToList();
            range.Should().Equal(0, 1, 2);
        }
    }
}
