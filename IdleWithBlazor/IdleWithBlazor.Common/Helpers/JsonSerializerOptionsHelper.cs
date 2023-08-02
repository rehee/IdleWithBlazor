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
      option.Converters.Add(new ITypedJsonConverter<IGameItem>());
      option.Converters.Add(new ITypedJsonConverter<IEquipment>());
      option.Converters.Add(new ITypedJsonConverter<IGameMap>());
      option.Converters.Add(new ITypedJsonConverter<IGameRoom>());
      option.Converters.Add(new ITypedJsonConverter<IPlayer>());
      option.Converters.Add(new ITypedJsonConverter<ISprite>());
      option.Converters.Add(new ITypedJsonConverter<ICharacter>());
      option.Converters.Add(new ITypedJsonConverter<IEquiptor>());
      option.Converters.Add(new ITypedJsonConverter<IActionSkill>());
      option.Converters.Add(new ITypedJsonConverter<IActionSlot>());

    }
  }
}
