using IdleWithBlazor.Common.Helpers;
using IdleWithBlazor.Common.Interfaces.Actors;
using IdleWithBlazor.Common.Interfaces.GameActions;
using IdleWithBlazor.Model.Actions;

namespace IdleWithBlazor.Model.Actors
{
  public class Monster : Sprite, IMonster
  {
    public override Type TypeDiscriminator => typeof(Monster);


    public override void Dispose()
    {
      base.Dispose();
      if (slots != null)
      {
        foreach (var slot in ActionSlots)
        {
          slot.Dispose();
        }
        slots.Clear();
        slots = null;
      }
    }
    public override void Init(IActor? parent, params object[] setInfo)
    {
      base.Init(parent, setInfo);

      var skills = setInfo.Where(b => b is IActionSkill).Select(b =>
      {
        if (b is IActionSkill action)
        {
          return (IActionSkill?)action;
        }
        return null;
      }).ToArray();

      if (skills?.Any() == true)
      {
        if (slots == null)
        {
          slots = new System.Collections.Concurrent.ConcurrentDictionary<int, IActionSlot>();
        }
        for (var i = 0; i < skills.Length; i++)
        {
          var slot = ActorHelper.New<IActionSlot>();
          slot.Init(this, skills[0]);
          if (!slots.TryAdd(i, slot))
          {
            slot.Dispose();
            slot = null;
          }
        }
      }
      skills = null;
    }
  }
}
