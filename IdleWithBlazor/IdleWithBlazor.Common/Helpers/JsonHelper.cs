using IdleWithBlazor.Common.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IdleWithBlazor.Common.Helpers
{
  public static class JsonHelper
  {
    public static string ToJson(object obj)
    {
      return JsonSerializer.Serialize(obj, ConstSetting.Options);
    }
    public static T? ToObject<T>(string json)
    {
      return JsonSerializer.Deserialize<T>(json, ConstSetting.Options);
    }
  }
}
