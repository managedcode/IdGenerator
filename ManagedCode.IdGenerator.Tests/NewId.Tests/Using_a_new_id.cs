using System;
using Xunit;

namespace ManagedCode.IdGenerator.Tests.NewId.Tests;

public class Using_a_new_id
{
    [Fact]
    public void Should_format_just_like_a_default_guid_formatter()
    {
        var newId = new IdGenerator.NewId.NewId();

        Assert.Equal("00000000-0000-0000-0000-000000000000", newId.ToString());
    }

    [Fact]
    public void Should_format_just_like_a_fancy_guid_formatter()
    {
        var newId = new IdGenerator.NewId.NewId();

        Assert.Equal("{00000000-0000-0000-0000-000000000000}", newId.ToString("B"));
    }

    [Fact]
    public void Should_format_just_like_a_narrow_guid_formatter()
    {
        var newId = new IdGenerator.NewId.NewId();

        Assert.Equal("00000000000000000000000000000000", newId.ToString("N"));
    }

    [Fact]
    public void Should_format_just_like_a_parenthesis_guid_formatter()
    {
        var newId = new IdGenerator.NewId.NewId();

        Assert.Equal("(00000000-0000-0000-0000-000000000000)", newId.ToString("P"));
    }

    [Fact]
    public void Should_work_from_guid_to_newid_to_guid()
    {
        var g = Guid.NewGuid();

        var n = new IdGenerator.NewId.NewId(g.ToByteArray());

        var gs = g.ToString("d");
        var ns = n.ToString("d");

        Assert.Equal(gs, ns);
    }
}