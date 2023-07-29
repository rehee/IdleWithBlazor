using IdleWithBlazor.Common.Interfaces.Actors;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace IdleWithBlazor.Common.Jsons.Converters
{
  public class ActionJsonConverter : JsonConverter<IActor>
  {
    public override IActor? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
      using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
      {
        JsonElement root = doc.RootElement;

        // Read the TypeDiscriminator value from the JSON
        if (root.TryGetProperty(nameof(IActor.TypeDiscriminator), out JsonElement typeProperty) && typeProperty.ValueKind == JsonValueKind.String)
        {
          string typeDiscriminator = typeProperty.GetString();
          var type = Type.GetType(typeDiscriminator);
          if (type != null)
          {
            // Deserialize the JSON to the concrete type
            return JsonSerializer.Deserialize(root, type, options) as IActor;
          }
        }

        throw new JsonException("Invalid JSON format for IAction.");
      }
    }

    public override void Write(Utf8JsonWriter writer, IActor value, JsonSerializerOptions options)
    {
      JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
  }
}
