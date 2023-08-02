using IdleWithBlazor.Common.Helpers;
using IdleWithBlazor.Common.Interfaces.Actors;
using IdleWithBlazor.Common.Interfaces.GameActions;
using IdleWithBlazor.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Model.Actions
{
  public class ActionSkill : GameAction, IActionSkill
  {
    public ActionSkill()
    {

    }
    public ActionSkill(IActionSkillInfo info)
    {
      Init(info);
    }
    public override Type TypeDiscriminator => typeof(ActionSkill);
    public decimal DamageRate { get; set; }
    public decimal CoolDown { get; set; }
    public bool GlobalCoolDown { get; set; }
    public decimal AttackSpeedRate { get; set; }
    public int TargetNumber { get; set; }
    public bool IsChain { get; set; }
    public bool ReChainTarget { get; set; }
    public decimal DamageChainReduction { get; set; }

    public Task<bool> Attack(ISprite attacker, IEnumerable<ISprite> targets)
    {
      if (attacker == null || targets?.Any() != true)
      {
        return Task.FromResult(false);
      }
      var liveSprite = targets.Where(b => b.CurrentHp > 0).ToList();
      if (liveSprite?.Any() != true)
      {
        return Task.FromResult(false); ;
      }
      if (attacker.MinAttack <= 0)
      {
        attacker.MinAttack = 1;
      }
      if (attacker.MaxAttack <= 0)
      {
        attacker.MaxAttack = 1;
      }
      if (attacker.MinAttack > attacker.MaxAttack)
      {
        attacker.MaxAttack = attacker.MinAttack;
      }
      var rawDamage = ItemHelper.GetRandomBetween(attacker.MinAttack, attacker.MaxAttack);

      var damageWithMultiple = new BigInteger(rawDamage * DamageRate);
      if (IsChain)
      {
        var damageChainReduction = Convert.ToInt32(DamageChainReduction * 100);
        AttackHelper.ChainDamage(TargetNumber, rawDamage, damageChainReduction, ReChainTarget, liveSprite);
      }
      else
      {
        foreach (var t in liveSprite.Take(TargetNumber))
        {
          t.CurrentHp = t.CurrentHp - damageWithMultiple;
        }
      }


      return Task.FromResult(false); ;
    }

    public void Init(IActionSkillInfo info)
    {
      this.Name = info.Name;
      this.Id = info.Id;
      this.DamageRate = info.DamageRate;
      this.CoolDown = info.CoolDown;
      this.GlobalCoolDown = info.GlobalCoolDown;
      this.AttackSpeedRate = info.AttackSpeedRate;
      this.TargetNumber = info.TargetNumber;
      this.IsChain = info.IsChain;
      this.ReChainTarget = info.ReChainTarget;
      this.DamageChainReduction = info.DamageChainReduction;
    }
  }
}
