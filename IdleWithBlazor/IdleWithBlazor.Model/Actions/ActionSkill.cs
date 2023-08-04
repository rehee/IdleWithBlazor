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
      Id = info.Id;
      Init(null, info);
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
        AttackHelper.ChainDamage(TargetNumber, damageWithMultiple, damageChainReduction, ReChainTarget, liveSprite);
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
    public override void Init(IActor? parent, params object[] setInfo)
    {
      base.Init(null);
      foreach (var info in setInfo)
      {
        if (info is IActionSkillInfo actionInfo)
        {
          this.Name = actionInfo.Name;
          this.Id = actionInfo.Id;
          this.DamageRate = actionInfo.DamageRate;
          this.CoolDown = actionInfo.CoolDown;
          this.GlobalCoolDown = actionInfo.GlobalCoolDown;
          this.AttackSpeedRate = actionInfo.AttackSpeedRate;
          this.TargetNumber = actionInfo.TargetNumber;
          this.IsChain = actionInfo.IsChain;
          this.ReChainTarget = actionInfo.ReChainTarget;
          this.DamageChainReduction = actionInfo.DamageChainReduction;
        }
      }
    }

  }
}
