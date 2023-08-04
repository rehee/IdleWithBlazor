using IdleWithBlazor.Common.Helpers;
using System.Text.Json;

namespace IdleWithBlazor.Common.Consts
{
  public class ConstSetting
  {
    public const int TickTime = 100;
    public const int SkillLimit = 6;
    public static JsonSerializerOptions Options => JsonSerializerOptionsHelper.Default;


  }
}
