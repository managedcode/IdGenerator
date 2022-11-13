using System;
using System.Data.SqlTypes;
using ManagedCode.IdGenerator.NewId;
using ManagedCode.IdGenerator.NewId.NewIdProviders;
using Xunit;

namespace ManagedCode.IdGenerator.Tests.NewId.Tests;

public class Generating_ids_over_time
{
    [Fact]
    public void Should_keep_them_ordered_for_sql_server()
    {
        var generator = new NewIdGenerator(new TimeLapseTickProvider(), new NetworkAddressWorkerIdProvider());
        generator.Next();

        var limit = 1024;

        var ids = new IdGenerator.NewId.NewId[limit];
        for (var i = 0; i < limit; i++)
        {
            ids[i] = generator.Next();
        }

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

    private class TimeLapseTickProvider :
        ITickProvider
    {
        private TimeSpan _interval = TimeSpan.FromSeconds(2);
        private DateTime _previous = DateTime.UtcNow;

        public long Ticks
        {
            get
            {
                _previous = _previous + _interval;
                _interval = TimeSpan.FromSeconds((long)_interval.TotalSeconds + 30);
                return _previous.Ticks;
            }
        }
    }
}