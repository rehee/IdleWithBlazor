using IdleWithBlazor.Common.Interfaces;
using IdleWithBlazor.Common.Interfaces.Actors;
using IdleWithBlazor.Common.Interfaces.GameActions;
using IdleWithBlazor.Common.Interfaces.Items;
using IdleWithBlazor.Common.Jsons.Converters;
using System.Text.Json;
using System.Text.Json.Serialization;

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
      option.Converters.Add(new BigIntegerJsonConverter());
      option.Converters.Add(new TypeJsonConverter());
      option.Converters.Add(new JsonStringEnumConverter());
      option.Converters.Add(new ActionJsonConverter());
      option.AddITypedJsonConvert<IGameItem>();
      option.AddITypedJsonConvert<IEquipment>();
      option.AddITypedJsonConvert<IGameMap>();
      option.AddITypedJsonConvert<IGameRoom>();
      option.AddITypedJsonConvert<IPlayer>();
      option.AddITypedJsonConvert<ISprite>();
      option.AddITypedJsonConvert<ICharacter>();
      option.AddITypedJsonConvert<IActionSkill>();
      option.AddITypedJsonConvert<IActionSlot>();
      option.AddITypedJsonConvert<IInventory>();
    }

    public static void AddITypedJsonConvert<T>(this JsonSerializerOptions option) where T : ITyped
    {
      option.Converters.Add(new ITypedJsonConverter<T>());
    }
  }
}
