﻿using IdleWithBlazor.Common.Helpers;
using IdleWithBlazor.Model.Actions;
using IdleWithBlazor.Model.Actors;

namespace IdleWithBlazor.Test.Models.Actions
{
  public class AttackTest
  {
    [TestCase(1, 1)]
    [TestCase(2, 1.1)]
    [TestCase(3, 2.1)]
    [TestCase(4, 3.5)]
    [TestCase(5, 4.1)]
    public async Task Attack_OnTick_Test(int index, decimal attackSpeed)
    {
      var tick = TickHelper.GetColdDownTick(attackSpeed, null);
      var attack = new Attack();
      attack.Init(attackSpeed);
      var player = new Player();
      var mob = new Monster();
      player.SetTarget(mob);
      attack.SetParent(player);
      var result = await attack.OnTick(null);
      Assert.IsTrue(result);
      for (var i = 0; i < tick; i++)
      {
        result = await attack.OnTick(null);
        Assert.IsFalse(result);
      }
      result = await attack.OnTick(null);
      Assert.IsTrue(result);
    }
  }
}
