using IdleWithBlazor.Common.Helpers;
using IdleWithBlazor.Common.Interfaces.Actors;
using IdleWithBlazor.Common.Interfaces.GameActions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Test.Helpers
{
  public class AttackHelperTest : TestBase
  {
    [Test]
    public void AttackHelper_Chain_Attack_Test()
    {
      var monsters = new List<ISprite>();
      for (var i = 0; i < 10; i++)
      {
        var mob = ActorHelper.New<IMonster>();
        mob.CurrentHp = 100;
        mob.MaxHp = 100;
        monsters.Add(mob);
      };
      AttackHelper.ChainDamage(10, 20, 100, false, monsters);
      foreach (var mob in monsters)
      {
        Assert.IsTrue(mob.CurrentHp == 80);
      }

    }
    [Test]
    public void AttackHelper_Chain_Attack_NoHit_Less_than_0Test()
    {
      var monsters = new List<ISprite>();
      for (var i = 0; i < 10; i++)
      {
        var mob = ActorHelper.New<IMonster>();
        mob.CurrentHp = -100;
        mob.MaxHp = 100;
        monsters.Add(mob);
      };
      AttackHelper.ChainDamage(10, 20, 100, false, monsters);
      foreach (var mob in monsters)
      {
        Assert.IsTrue(mob.CurrentHp == -100);
      }

    }
    [Test]
    public void AttackHelper_Chain_Attack_Hit_Correct_Number_Test()
    {
      var monsters = new List<ISprite>();
      for (var i = 0; i < 10; i++)
      {
        var mob = ActorHelper.New<IMonster>();
        mob.CurrentHp = 100;
        mob.MaxHp = 100;
        monsters.Add(mob);
      };
      AttackHelper.ChainDamage(5, 20, 100, false, monsters);
      foreach (var mob in monsters.Where(b => b.CurrentHp != 100))
      {
        Assert.IsTrue(mob.CurrentHp == 80);
      }
      Assert.That(monsters.Count(b => b.CurrentHp != 100), Is.EqualTo(5));
      Assert.That(monsters.Count(b => b.CurrentHp == 100), Is.EqualTo(5));
    }
  }
}
