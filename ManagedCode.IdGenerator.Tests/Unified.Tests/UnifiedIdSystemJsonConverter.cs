using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using ManagedCode.IdGenerator.Unified;

namespace ManagedCode.IdGenerator.Tests.Unified.Tests;

public class UnifiedIdSystemJsonConverter : JsonConverter<UnifiedId>
{
    public override UnifiedId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return UnifiedId.Parse(reader.GetString());
    }

    public override void Write(Utf8JsonWriter writer, UnifiedId value, JsonSerializerOptions options)
    {
        writer?.WriteStringValue(value.ToString());
    }
}