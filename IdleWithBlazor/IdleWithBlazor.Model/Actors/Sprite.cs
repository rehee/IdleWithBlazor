using IdleWithBlazor.Model.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Model.Actors
{
  public class Sprite : Actor
  {
    public virtual BigInteger MaxHp { get; set; }
    public virtual BigInteger CurrentMp { get; set; }
    public virtual IActor Target { get; protected set; }
    public virtual void SetTarget(IActor target)
    {
      Target = target;
    }
    public virtual void SetAction(IActionSkill skill)
    {
      this.ActionSkills = new List<IActionSkill>() { skill };
      skill.SetActor(this);
    }

    public virtual IEnumerable<IActionSkill> ActionSkills { get; set; }

    public override async Task OnTick()
    {
      await base.OnTick();
      if (ActionSkills?.Any() == true)
      {
        foreach (var skill in ActionSkills)
        {
          skill.OnTick();
        }
      }
    }

  }
}
