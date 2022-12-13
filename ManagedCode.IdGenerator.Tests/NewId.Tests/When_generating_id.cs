using System;
using System.Diagnostics;
using ManagedCode.IdGenerator.NewId;
using Xunit;

namespace ManagedCode.IdGenerator.Tests.NewId.Tests;

public class When_generating_id
{
    private IProcessIdProvider _processIdProvider;
    private DateTime _start;
    private Stopwatch _stopwatch;

    private ITickProvider _tickProvider;
    private IWorkerIdProvider _workerIdProvider;
    
    public When_generating_id()
    {
        _start = DateTime.UtcNow;
        _stopwatch = Stopwatch.StartNew();

        _tickProvider = new MockTickProvider(GetTicks());
        _workerIdProvider = new MockNetworkProvider(BitConverter.GetBytes(1234567890L));
        _processIdProvider = new MockProcessIdProvider(BitConverter.GetBytes(10));
    }
    
    [Fact]
    public void Should_match_when_all_providers_equal()
    {
        // Arrange
        var generator1 = new NewIdGenerator(_tickProvider, _workerIdProvider, _processIdProvider);
        var generator2 = new NewIdGenerator(_tickProvider, _workerIdProvider, _processIdProvider);

        // Act
        var id1 = generator1.Next();
        var id2 = generator2.Next();

        // Assert
        Assert.Equal(id1, id2);
    }

    [Fact]
    public void Should_match_when_all_providers_equal_with_guid_method()
    {
        // Arrange
        var generator1 = new NewIdGenerator(_tickProvider, _workerIdProvider, _processIdProvider);
        var generator2 = new NewIdGenerator(_tickProvider, _workerIdProvider, _processIdProvider);
        generator1.Next().ToGuid();
        generator2.NextGuid();

        // Act
        var id1 = generator1.Next().ToGuid();
        var id2 = generator2.NextGuid();

        // Assert
        Assert.Equal(id1, id2);
    }

    [Fact]
    public void Should_not_match_when_generated_from_two_processes()
    {
        // Arrange
        var generator1 = new NewIdGenerator(_tickProvider, _workerIdProvider, _processIdProvider);

        _processIdProvider = new MockProcessIdProvider(BitConverter.GetBytes(11));
        var generator2 = new NewIdGenerator(_tickProvider, _workerIdProvider, _processIdProvider);

        // Act
        var id1 = generator1.Next();
        var id2 = generator2.Next();

        // Assert
        Assert.NotEqual(id1, id2);
    }

    [Fact]
    public void Should_not_match_when_processor_id_provided()
    {
        // Arrange
        var generator1 = new NewIdGenerator(_tickProvider, _workerIdProvider);
        var generator2 = new NewIdGenerator(_tickProvider, _workerIdProvider, _processIdProvider);

        // Act
        var id1 = generator1.Next();
        var id2 = generator2.Next();

        // Assert
        Assert.NotEqual(id1, id2);
    }

    [Fact]
    public void Should_match_sequentially()
    {
        var generator = new NewIdGenerator(_tickProvider, _workerIdProvider);

        var id1 = generator.Next().ToGuid();
        var id2 = generator.NextGuid();
        var id3 = generator.NextGuid();

        Assert.NotEqual(id1, id2);
        Assert.NotEqual(id2, id3);
        Assert.True(id2 > id1);

        Console.WriteLine(id1);
        Console.WriteLine(id2);
        Console.WriteLine(id3);

        var nid1 = id1.ToNewId();
        var nid2 = id2.ToNewId();
    }

    [Fact]
    public void Should_match_sequentially_with_sequential_guid()
    {
        var generator = new NewIdGenerator(_tickProvider, _workerIdProvider);

        var nid = generator.Next();
        var id1 = nid.ToSequentialGuid();
        var id2 = generator.NextSequentialGuid();
        var id3 = generator.NextSequentialGuid();

        Assert.NotEqual(id1, id2);
        Assert.NotEqual(id2, id3);
        Assert.True(id2 > id1);

        Console.WriteLine(id1);
        Console.WriteLine(id2);
        Console.WriteLine(id3);

        var nid1 = id1.ToNewIdFromSequential();
        var nid2 = id2.ToNewIdFromSequential();

        Assert.Equal(nid, nid1);
    }

    private long GetTicks()
    {
        return _start.AddTicks(_stopwatch.Elapsed.Ticks).Ticks;
    }

    private class MockTickProvider :
        ITickProvider
    {
        public MockTickProvider(long ticks)
        {
            Ticks = ticks;
        }

        public long Ticks { get; }
    }

    private class MockNetworkProvider :
        IWorkerIdProvider
    {
        private readonly byte[] _workerId;

        public MockNetworkProvider(byte[] workerId)
        {
            _workerId = workerId;
        }

        public byte[] GetWorkerId(int index)
        {
            return _workerId;
        }
    }

    private class MockProcessIdProvider :
        IProcessIdProvider
    {
        private readonly byte[] _processId;

        public MockProcessIdProvider(byte[] processId)
        {
            _processId = processId;
        }

        public byte[] GetProcessId()
        {
            return _processId;
        }
    }
}