// using System;
// using System.Collections.Generic;
// using IdGen;
// using IdGen.Configuration;
// using Microsoft.VisualStudio.TestTools.UnitTesting;
// using Xunit;
//
// namespace IdGenTests;
//
// public class ConfigTests
// {
//     [Fact]
//     public void IdGenerator_GetFromConfig_CreatesCorrectGenerator1()
//     {
//         var target = AppConfigFactory.GetFromConfig("foo");
//
//         Assert.Equal(123, target.Id);
//         Assert.Equal(new DateTime(2016, 1, 2, 12, 34, 56, DateTimeKind.Utc), target.Options.TimeSource.Epoch);
//         Assert.Equal(39, target.Options.IdStructure.TimestampBits);
//         Assert.Equal(11, target.Options.IdStructure.GeneratorIdBits);
//         Assert.Equal(13, target.Options.IdStructure.SequenceBits);
//         Assert.Equal(TimeSpan.FromMilliseconds(50), target.Options.TimeSource.TickDuration);
//         Assert.Equal(SequenceOverflowStrategy.Throw, target.Options.SequenceOverflowStrategy);
//     }
//
//     [Fact]
//     public void IdGenerator_GetFromConfig_CreatesCorrectGenerator2()
//     {
//         var target = AppConfigFactory.GetFromConfig("baz");
//
//         Assert.Equal(2047, target.Id);
//         Assert.Equal(new DateTime(2016, 2, 29, 0, 0, 0, DateTimeKind.Utc), target.Options.TimeSource.Epoch);
//         Assert.Equal(21, target.Options.IdStructure.TimestampBits);
//         Assert.Equal(21, target.Options.IdStructure.GeneratorIdBits);
//         Assert.Equal(21, target.Options.IdStructure.SequenceBits);
//         Assert.Equal(TimeSpan.FromTicks(7), target.Options.TimeSource.TickDuration);
//         Assert.Equal(SequenceOverflowStrategy.SpinWait, target.Options.SequenceOverflowStrategy);
//     }
//
//     [Fact]
//     [ExpectedException(typeof(KeyNotFoundException))]
//     public void IdGenerator_GetFromConfig_IsCaseSensitive() => AppConfigFactory.GetFromConfig("Foo");
//
//     [Fact]
//     [ExpectedException(typeof(KeyNotFoundException))]
//     public void IdGenerator_GetFromConfig_ThrowsOnNonExisting() => AppConfigFactory.GetFromConfig("xxx");
//
//
//     [Fact]
//     [ExpectedException(typeof(InvalidOperationException))]
//     public void IdGenerator_GetFromConfig_ThrowsOnInvalidIdStructure() => AppConfigFactory.GetFromConfig("e1");
//
//     [Fact]
//     [ExpectedException(typeof(FormatException))]
//     public void IdGenerator_GetFromConfig_ThrowsOnInvalidEpoch() => AppConfigFactory.GetFromConfig("e2");
//
//     [Fact]
//     public void IdGenerator_GetFromConfig_ReturnsSameInstanceForSameName()
//     {
//         var target1 = AppConfigFactory.GetFromConfig("foo");
//         var target2 = AppConfigFactory.GetFromConfig("foo");
//
//         Assert.True(ReferenceEquals(target1, target2));
//     }
//
//     [Fact]
//     public void IdGenerator_GetFromConfig_ParsesEpochCorrectly()
//     {
//         Assert.Equal(new DateTime(2016, 1, 2, 12, 34, 56, DateTimeKind.Utc), AppConfigFactory.GetFromConfig("foo").Options.TimeSource.Epoch);
//         Assert.Equal(new DateTime(2016, 2, 1, 1, 23, 45, DateTimeKind.Utc), AppConfigFactory.GetFromConfig("bar").Options.TimeSource.Epoch);
//         Assert.Equal(new DateTime(2016, 2, 29, 0, 0, 0, DateTimeKind.Utc), AppConfigFactory.GetFromConfig("baz").Options.TimeSource.Epoch);
//     }
//
//     [Fact]
//     public void IdGenerator_GetFromConfig_ParsesTickDurationCorrectly()
//     {
//         Assert.Equal(TimeSpan.FromMilliseconds(50), AppConfigFactory.GetFromConfig("foo").Options.TimeSource.TickDuration);
//         Assert.Equal(new TimeSpan(1, 2, 3), AppConfigFactory.GetFromConfig("bar").Options.TimeSource.TickDuration);
//         Assert.Equal(TimeSpan.FromTicks(7), AppConfigFactory.GetFromConfig("baz").Options.TimeSource.TickDuration);
//
//         // Make sure the default tickduration is 1 ms
//         Assert.Equal(TimeSpan.FromMilliseconds(1), AppConfigFactory.GetFromConfig("nt").Options.TimeSource.TickDuration);
//     }
//
//     [Fact]
//     public void IdGeneratorElement_Property_Setters()
//     {
//         // We create an IdGeneratorElement from code and compare it to an IdGeneratorElement from config.
//         var target = new IdGeneratorElement()
//         {
//             Name = "newfoo",
//             Id = 123,
//             Epoch = new DateTime(2016, 1, 2, 12, 34, 56, DateTimeKind.Utc),
//             TimestampBits = 39,
//             GeneratorIdBits = 11,
//             SequenceBits = 13,
//             TickDuration = TimeSpan.FromMilliseconds(50),
//             SequenceOverflowStrategy = SequenceOverflowStrategy.Throw
//         };
//         var expected = AppConfigFactory.GetFromConfig("foo");
//
//         Assert.Equal(expected.Id, target.Id);
//         Assert.Equal(expected.Options.TimeSource.Epoch, target.Epoch);
//         Assert.Equal(expected.Options.IdStructure.TimestampBits, target.TimestampBits);
//         Assert.Equal(expected.Options.IdStructure.GeneratorIdBits, target.GeneratorIdBits);
//         Assert.Equal(expected.Options.IdStructure.SequenceBits, target.SequenceBits);
//         Assert.Equal(expected.Options.TimeSource.TickDuration, target.TickDuration);
//         Assert.Equal(expected.Options.SequenceOverflowStrategy, target.SequenceOverflowStrategy);
//     }
// }