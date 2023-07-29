using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IdleWithBlazor.Common.Jsons.Converters
{
  public class TypeJsonConverter : JsonConverter<Type>
  {
    public override Type? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
      var str = reader.GetString();
      if (String.IsNullOrEmpty(str))
      {
        throw new Exception("Not Able Convert Type");
      }
      try
      {
        var type = Type.GetType(str);
        if (type == null)
        {
          return type;
        }
      }
      catch
      {

      }
      throw new Exception("Not Able Convert Type");
    }

    public override void Write(Utf8JsonWriter writer, Type value, JsonSerializerOptions options)
    {
      writer.WriteStringValue($@"{value.FullName}, {value.Assembly.FullName.Split(",").Select(b => b.Trim()).FirstOrDefault()}");
    }
  }
}
