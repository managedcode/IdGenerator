using System.Text.Json;
using System.Text.Json.Serialization;

namespace ManagedCode.IdGenerator.Unified;

/// <summary>
///     UnifiedIdConverter.
/// </summary>
public class UnifiedIdConverter : JsonConverter<UnifiedId>
{
    public override UnifiedId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return UnifiedId.Parse(reader.GetString()!);
    }

    public override void Write(Utf8JsonWriter writer, UnifiedId value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}