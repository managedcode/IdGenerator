using ManagedCode.IdGenerator.IdGen;
using Xunit;

namespace IdGenTests;

public class IDTests
{
    [Fact]
    public void ID_DoesNotEqual_RandomObject()
    {
        var g = new IdGenerator(0);
        var i = g.FromId(0);
        Assert.False(i.Equals(new object()));
        Assert.True(i.Equals((object)g.FromId(0)));
        Assert.True(i != g.FromId(1));
        Assert.True(i == g.FromId(0));
        Assert.Equal(i.GetHashCode(), g.FromId(0).GetHashCode());
    }

    [Fact]
    public void ID_Equals_OtherId()
    {
        var g = new IdGenerator(0);
        var i = g.FromId(1234567890);
        Assert.True(i.Equals(g.FromId(1234567890)));
        Assert.True(i.Equals((object)g.FromId(1234567890)));
        Assert.True(i != g.FromId(0));
        Assert.True(i == g.FromId(1234567890));
        Assert.Equal(i.GetHashCode(), g.FromId(1234567890).GetHashCode());
    }

    [Fact]
    public void ID_FromZeroInt_HasCorrectValue()
    {
        var g = new IdGenerator(0);
        var i = g.FromId(0);

        Assert.Equal(0, i.SequenceNumber);
        Assert.Equal(0, i.GeneratorId);
        Assert.Equal(g.Options.TimeSource.Epoch, i.DateTimeOffset);
    }


    [Fact]
    public void ID_FromOneInt_HasCorrectValue()
    {
        var g = new IdGenerator(0);
        var i = g.FromId(1);

        Assert.Equal(1, i.SequenceNumber);
        Assert.Equal(0, i.GeneratorId);
        Assert.Equal(g.Options.TimeSource.Epoch, i.DateTimeOffset);
    }

}
