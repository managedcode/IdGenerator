using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using ManagedCode.IdGenerator.ConsistentHash;
using ManagedCode.IdGenerator.ConsistentHashing;
using Xunit;
using Xunit.Abstractions;

namespace ManagedCode.IdGenerator.Tests;

public class GeneralTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public GeneralTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    private string[] _servers = Enumerable.Range(1, 50).Select(s => "server" + s).ToArray();

    [Fact]
    public void ConsistentHashing()
    {
        Dictionary<string, int> result = new Dictionary<string, int>();
        var hash = new ConsistentHash.ConsistentHash<string>();
        hash.Init(_servers);

        for (int i = 0; i < 10_000; i++)
        {
            var node = hash.GetNode(i.ToString());
            if (result.ContainsKey(node))
            {
                result[node]++;
            }
            else
            {
                result[node] = 1;
            }
        }

        result.Count.Should().Be(50);
    }
    
    [Fact]
    public void HashRing()
    {
        Dictionary<string, int> result = new Dictionary<string, int>();
        var hash = new HashRing<string>();
        uint number = 1;
        foreach (var item in _servers)
        {
            hash.AddNode(item, number);
            number++;
        }
        

        for (int i = 0; i < 10_000; i++)
        {
            var node = hash.GetNode(Convert.ToUInt32(i));
            if (result.ContainsKey(node))
            {
                result[node]++;
            }
            else
            {
                result[node] = 1;
            }
        }
        
        result.Count.Should().Be(50);
    }
    
    [Fact]
    public void ConsistentSharp()
    {
        Dictionary<string, int> result = new Dictionary<string, int>();
        var hash = new ManagedCode.IdGenerator.ConsistentSharp.ConsistentHash();
        uint number = 1;
        foreach (var item in _servers)
        {
            hash.Add(item);
            number++;
        }
        
        for (int i = 0; i < 10_000; i++)
        {
            var node = hash.Get(i.ToString());
            if (result.ContainsKey(node))
            {
                result[node]++;
            }
            else
            {
                result[node] = 1;
            }
        }
        
        result.Count.Should().Be(50);
    }

}