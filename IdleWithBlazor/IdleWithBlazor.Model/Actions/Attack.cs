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
    public override Type TypeDiscriminator => typeof(Attack);
    public override Decimal CoolDown { get; set; }
    public override Decimal AttackSpeed { get; set; }

    public virtual void Init(decimal attackSpeed)
    {
      CoolDown = 0;
      AttackSpeed = attackSpeed;
      SetCooldownTick();
    }

    public override async Task<bool> OnTick()
    {
      var isOntick = await base.OnTick();
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
        lock (sprite)
        {
          if (sprite.CurrentHp == 0)
          {
            sprite.CurrentHp = sprite.MaxHp;
            return true;
          }
          sprite.CurrentHp = sprite.CurrentHp - 1;
        }
        return true;
      }

      return false;
    }

  }
}
