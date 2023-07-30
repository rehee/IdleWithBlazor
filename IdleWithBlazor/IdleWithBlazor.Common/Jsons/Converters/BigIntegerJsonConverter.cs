﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IdleWithBlazor.Common.Jsons.Converters
{
  public class BigIntegerJsonConverter : JsonConverter<BigInteger>
  {
    public override BigInteger Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
      var str = reader.GetString();
      try
      {
        if (BigInteger.TryParse(str, out var value))
        {
          return value;
        }
      }
      catch
      {
        
      }
      throw new JsonException($"Unable to convert JSON value to BigInteger.");

    }

    public override void Write(Utf8JsonWriter writer, BigInteger value, JsonSerializerOptions options)
    {
      writer.WriteStringValue(value.ToString());
    }
  }
}