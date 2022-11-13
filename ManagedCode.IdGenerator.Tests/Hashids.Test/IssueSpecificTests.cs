using System;
using System.Collections.Generic;
using FluentAssertions;
using ManagedCode.IdGenerator.Hashids;
using Xunit;

namespace ManagedCode.IdGenerator.Tests.Hashids.Test;

public class IssueSpecificTests
{
    [Fact]
    private void Issue_8_should_not_throw_out_of_range_exception()
    {
        var hashids = new IdGenerator.Hashids.Hashids("janottaa", 6);
        var numbers = hashids.Decode("NgAzADEANAA=");
    }

    // This issue came from downcasting to int at the wrong place,
    // seems to happen when you are encoding A LOT of longs at the same time.
    // see if it is possible to make this a faster test (or remove it since it is unlikely that it will reapper).
    [Fact]
    private void Issue_12_should_not_throw_out_of_range_exception()
    {
        var hash = new IdGenerator.Hashids.Hashids("zXZVFf2N38uV");
        var longs = new List<long>();
        var rand = new Random();
        var valueBuffer = new byte[8];
        var randLong = 0L;
        for (var i = 0; i < 100000; i++)
        {
            rand.NextBytes(valueBuffer);
            randLong = BitConverter.ToInt64(valueBuffer, 0);
            longs.Add(Math.Abs(randLong));
        }

        var encoded = hash.EncodeLong(longs);
        var decoded = hash.DecodeLong(encoded);
        decoded.Should().Equal(longs.ToArray());
    }

    [Fact]
    private void Issue_15_it_should_return_empty_array_when_decoding_characters_missing_in_alphabet()
    {
        var hashids = new IdGenerator.Hashids.Hashids("Salty stuff", alphabet: "qwerty1234!¤%&/()=", seps: "1234");
        var numbers = hashids.Decode("abcd");
        numbers.Length.Should().Be(0);

        var hashids2 = new IdGenerator.Hashids.Hashids();
        hashids2.Decode("13-37").Length.Should().Be(0);
        hashids2.DecodeLong("32323kldffd!").Length.Should().Be(0);

        var hashids3 = new IdGenerator.Hashids.Hashids(alphabet: "1234567890;:_!#¤%&/()=", seps: "!#¤%&/()=");
        hashids3.Decode("asdfb").Length.Should().Be(0);
        hashids3.DecodeLong("asdfgfdgdfgkj").Length.Should().Be(0);
    }

    [Fact]
    private void Issue_64_long_max_value_with_min_alphabet_length()
    {
        var hashids = new IdGenerator.Hashids.Hashids("salt", alphabet: "0123456789ABCDEF");
        var hash = hashids.EncodeLong(long.MaxValue);

        hash.Should().Be("58E9BDD9A7598254DA4E");

        var decoded = hashids.DecodeSingleLong(hash);

        decoded.Should().Be(long.MaxValue);
    }

    [Fact]
    private void Issue75_1CharacterHashShouldNotThrowException()
    {
        var hashids = new IdGenerator.Hashids.Hashids("salt");
        Assert.Throws<NoResultException>(() => hashids.DecodeSingle("a"));
    }

    [Fact]
    private void Issue75_TooShortHashShouldNotThrowException()
    {
        var hashids = new IdGenerator.Hashids.Hashids("salt");
        Assert.Throws<NoResultException>(() => hashids.DecodeSingle("ab"));
    }

    [Fact]
    private void Issue75_TooShortHashWithLargerHashLengthShouldNotThrowException()
    {
        var hashids = new IdGenerator.Hashids.Hashids("salt", 40);
        Assert.Throws<NoResultException>(() => hashids.DecodeSingle("ab"));
    }
}