using System;
using ManagedCode.IdGenerator.Unified;

namespace ManagedCode.IdGenerator.Tests.Unified.Tests;

[Serializable]
public class TestContract
{
    public UnifiedId Id { set; get; }
}