using ITNOte.me.Model.Notes;

namespace ITNOte.me.Model.Storage;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using ITNOte.me.Model.Notes;

public class AbstractSourceConverter : JsonConverter<AbstractSource>
{
    public override AbstractSource Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var document = JsonDocument.ParseValue(ref reader);
        var jsonObject = document.RootElement;

        if (!jsonObject.TryGetProperty("Type", out var typeProperty)) throw new JsonException("Unknown type");
        var type = typeProperty.GetString();
        return type switch
        {
            nameof(Folder) => JsonSerializer.Deserialize<Folder>(jsonObject.GetRawText(), options)!,
            nameof(Note) => JsonSerializer.Deserialize<Note>(jsonObject.GetRawText(), options)!,
            _ => throw new JsonException("Unknown type")
        };
    }

    public override void Write(Utf8JsonWriter writer, AbstractSource value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.Type.Equals(nameof(Folder)) ? typeof(Folder) : typeof(Note),
            options);
    }
}
