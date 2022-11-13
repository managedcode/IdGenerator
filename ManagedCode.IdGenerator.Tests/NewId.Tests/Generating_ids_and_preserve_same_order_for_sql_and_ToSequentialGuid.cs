﻿using System;
using System.Data.SqlTypes;
using System.Diagnostics;
using FluentAssertions;
using ManagedCode.IdGenerator.NewId;
using Xunit;

namespace ManagedCode.IdGenerator.Tests.NewId.Tests;

public class Generating_ids_and_preserve_same_order_for_sql_and_ToSequentialGuid
{
    [Fact]
    public void Should_keep_them_ordered_for_sql_server_when_using_array_call()
    {
        var generator = new NewIdGenerator(_tickProvider, _workerIdProvider);
        generator.Next();

        var limit = 1024;

        var ids = new IdGenerator.NewId.NewId[limit];
        generator.Next(ids, 0, limit);

        for (var i = 0; i < limit - 1; i++)
        {
            Assert.NotEqual(ids[i], ids[i + 1]);

            SqlGuid left = ids[i].ToGuid();
            SqlGuid right = ids[i + 1].ToGuid();
            //Assert.Less(left, right);
            Assert.True((left > right).Value);
            if (i % 128 == 0)
            {
                Console.WriteLine("Normal: {0} Sql: {1}", left, ids[i].ToSequentialGuid());
            }
        }
    }

    [Fact]
    public void Should_keep_them_ordered_for_ToSequentialGuid_when_using_array_call()
    {
        var generator = new NewIdGenerator(_tickProvider, _workerIdProvider);
        generator.Next();

        var limit = 1024;

        var ids = new IdGenerator.NewId.NewId[limit];
        generator.Next(ids, 0, limit);

        for (var i = 0; i < limit - 1; i++)
        {
            Assert.NotEqual(ids[i], ids[i + 1]);

            var left = ids[i].ToSequentialGuid();
            var right = ids[i + 1].ToSequentialGuid();
            //Assert.Less(left, right);
            Assert.True(left > right);
            if (i % 128 == 0)
            {
                Console.WriteLine("Sql: {0}", left);
            }
        }
    }

    #region ordering using MAC-address is currently not relevant

#if ORDERING_MAC_ADDRESS
        [Fact]
        public void Should_keep_them_ordered_for_sql_server_when_using_different_mac_addresses()
        {
            var generator1 = new NewIdGenerator(_tickProvider, new MockNetworkProvider(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, }));
            var generator2 = new NewIdGenerator(_tickProvider, new MockNetworkProvider(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, }));

            var id1 = generator1.Next();
            var id2 = generator2.Next();

            Assert.AreNotEqual(id1, id2);

            SqlGuid left = id1.ToGuid();
            SqlGuid right = id2.ToGuid();
            Assert.Less(left, right);
            Console.WriteLine("Normal: {0} Sql: {1}", left, id1.ToSequentialGuid());
            Console.WriteLine("Normal: {0} Sql: {1}", right, id2.ToSequentialGuid());
        }

        [Fact]
        public void Should_keep_them_ordered_for_ToSequentialGuid_when_using_different_mac_addresses()
        {
            var generator1 = new NewIdGenerator(_tickProvider, new MockNetworkProvider(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, }));
            var generator2 = new NewIdGenerator(_tickProvider, new MockNetworkProvider(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, }));

            var id1 = generator1.Next();
            var id2 = generator2.Next();

            Assert.AreNotEqual(id1, id2);

            Guid left = id1.ToSequentialGuid();
            Guid right = id2.ToSequentialGuid();
            Assert.Less(left, right);
            Console.WriteLine("Sql: {0}", left);
            Console.WriteLine("Sql: {0}", right);
        }

#endif

    #endregion

    #region ordering using process id is currently not relevant

#if ORDERING_PROCESS_ID
        [Fact]
        public void Should_keep_them_ordered_for_sql_server_when_using_different_processes()
        {
            var generator1 = new NewIdGenerator(_tickProvider, _workerIdProvider, new MockProcessIdProvider(BitConverter.GetBytes(1)));
            var generator2 = new NewIdGenerator(_tickProvider, _workerIdProvider, new MockProcessIdProvider(BitConverter.GetBytes(256)));

            var id1 = generator1.Next();
            var id2 = generator2.Next();

            Assert.AreNotEqual(id1, id2);

            SqlGuid left = id1.ToGuid();
            SqlGuid right = id2.ToGuid();
            Assert.Less(left, right);
            Console.WriteLine("Normal: {0} Sql: {1}", left, id1.ToSequentialGuid());
            Console.WriteLine("Normal: {0} Sql: {1}", right, id2.ToSequentialGuid());
        }

        [Fact]
        public void Should_keep_them_ordered_for_ToSequentialGuid_when_using_different_processes()
        {
            var generator1 = new NewIdGenerator(_tickProvider, _workerIdProvider, new MockProcessIdProvider(BitConverter.GetBytes(1)));
            var generator2 = new NewIdGenerator(_tickProvider, _workerIdProvider, new MockProcessIdProvider(BitConverter.GetBytes(256)));

            var id1 = generator1.Next();
            var id2 = generator2.Next();

            Assert.AreNotEqual(id1, id2);

            Guid left = id1.ToSequentialGuid();
            Guid right = id2.ToSequentialGuid();
            Assert.Less(left, right);
            Console.WriteLine("Sql: {0}", left);
            Console.WriteLine("Sql: {0}", right);
        }

#endif

    #endregion

   // [SetUp]
    public void Init()
    {
        _start = DateTime.UtcNow;
        _stopwatch = Stopwatch.StartNew();

        _tickProvider = new MockTickProvider(GetTicks());
        _workerIdProvider = new MockNetworkProvider(BitConverter.GetBytes(1234567890L));
    }

    private ITickProvider _tickProvider;
    private IWorkerIdProvider _workerIdProvider;
    private DateTime _start;
    private Stopwatch _stopwatch;

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