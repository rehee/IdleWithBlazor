using IdleWithBlazor.Common.Interfaces;
using IdleWithBlazor.Common.Interfaces.Actors;
using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace IdleWithBlazor.Common.Jsons.Converters
{
  public class ITypedJsonConverter<T> : JsonConverter<T> where T : ITyped
  {
    public static ConcurrentDictionary<string, Type> TypeNameMapper = new ConcurrentDictionary<string, Type>();
    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
      using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
      {
        JsonElement root = doc.RootElement;

        // Read the TypeDiscriminator value from the JSON
        if (root.TryGetProperty(nameof(IActor.TypeDiscriminator), out JsonElement typeProperty) && typeProperty.ValueKind == JsonValueKind.String)
        {
          string typeDiscriminator = typeProperty.GetString();
          if (!TypeNameMapper.TryGetValue(typeDiscriminator, out var savedType))
          {
            savedType = Type.GetType(typeDiscriminator);
            TypeNameMapper.TryAdd(typeDiscriminator, savedType);
          }
          if (savedType != null)
          {
            // Deserialize the JSON to the concrete type
            var obj = JsonSerializer.Deserialize(root, savedType, options);
            if (obj is T tType)
            {
              obj = null;
              return tType;
            }

          }
        }

        throw new JsonException("Invalid JSON format for IAction.");
      }
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
      JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
  }
}
