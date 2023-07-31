using IdleWithBlazor.Common.Jsons.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IdleWithBlazor.Common.Helpers
{
  public static class JsonSerializerOptionsHelper
  {
    private static JsonSerializerOptions? _options;
    public static object lockObj { get; set; } = new object();
    public static JsonSerializerOptions Default
    {
      get
      {
        if (_options != null)
        {
          return _options;
        }

        var options = new JsonSerializerOptions();
        options.SetDefaultOption();
        _options = options;
        return _options;

      }
    }


    public static void SetDefaultOption(this JsonSerializerOptions option)
    {
      option.Converters.Add(new ActionJsonConverter());
      option.Converters.Add(new BigIntegerJsonConverter());
      option.Converters.Add(new TypeJsonConverter());
      option.Converters.Add(new JsonStringEnumConverter());
    }
  }
}
