using IdleWithBlazor.Model.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Model.Actions
{
  public class Attack : ActionSkill
  {
    public override Decimal CoolDown { get; set; }
    public override Decimal AttackSpeed { get; set; }

    public virtual void Init(decimal attackSpeed)
    {
      CoolDown = 0;
      AttackSpeed = attackSpeed;
      SetCooldownTick();
    }

    public override bool OnTick()
    {
      var isOntick = base.OnTick();
      if (!isOntick)
      {
        return false;
      }
      var actor = ActorType<Sprite>();
      if (!actor.isType)
      {
        return false;
      }
      var target = actor.value.Target;
      if (target is Sprite sprite)
      {
        sprite.CurrentMp = sprite.CurrentMp - 1;
        return true;
      }

      return false;
    }

  }
}
