﻿using IdleWithBlazor.Common.Helpers;
using IdleWithBlazor.Common.Interfaces.Actors;
using System.Collections.Concurrent;
using System.Numerics;
using System.Text.Json.Serialization;

namespace IdleWithBlazor.Model.Actors
{
  public class GameMap : Actor, IGameMap
  {
    public override Type TypeDiscriminator => typeof(GameMap);
    public override IEnumerable<IActor> Children()
    {
      foreach (var player in Players())
      {
        yield return player;
      }
      if (monsters != null)
      {
        foreach (var mob in monsters)
        {
          yield return mob;
        }
      }

    }
    public IEnumerable<IPlayer> Players()
    {
      if (owner != null)
      {
        yield return owner?.ThisPlayer;
      }
      if (guests != null)
      {
        foreach (var g in guests())
        {
          yield return g.ThisPlayer;
        }
      }

    }
    public IEnumerable<IMonster?> Monsters => monsters ?? Enumerable.Empty<IMonster?>();

    private List<IMonster> monsters { get; set; }

    private ICharacter? owner { get; set; }
    private Func<IEnumerable<ICharacter>> guests { get; set; }

    public async Task<bool> CloseMapAsync()
    {
      this.owner = null;
      this.guests = null;
      await DisposeAsync();
      return true;
    }

    public Task<bool> GenerateMobsAsync()
    {
      if (!FirstRespawn && MonsterRespawn > 0)
      {
        MonsterRespawn--;
        return Task.FromResult(false);
      }
      FirstRespawn = false;
      MonsterRespawn = MonsterRespawnRate;
      if (monsters == null)
      {
        monsters = new List<IMonster>();
        for (var i = 0; i < 5; i++)
        {
          var m = ActorHelper.New<IMonster>();
          m.Name = "小野怪";
          m.CurrentHp = 10;
          m.MaxHp = 10;
          m.Level = 1;
          if (m != null)
          {
            monsters.Add(m);
          }
        }
      }
      //monsters.Clear();
      foreach (var m in monsters)
      {
        m.CurrentHp = m.MaxHp;
      }
      return Task.FromResult(true);
    }

    public override void Init(IActor? parent, params object[] setInfo)
    {
      base.Init(parent, setInfo);
      if (parent != null && parent is IGameRoom room)
      {
        this.owner = room.GameOwner;
        this.guests = () => room.Guests();
        MonsterRespawnRate = 50;
        MonsterRespawn = MonsterRespawnRate;
        FirstRespawn = true;
      }
    }

    private bool FirstRespawn { get; set; }
    public int MonsterRespawnRate { get; set; }
    public int MonsterRespawn { get; set; }
    public override async Task<bool> OnTick(IServiceProvider sp)
    {
      var monster = Monsters.ToList();
      if (monster.All(b => b.CurrentHp <= 0))
      {
        monster = null;
        return await GenerateMobsAsync();

      }
      var baseResult = await base.OnTick(sp);

      foreach (var player in Players())
      {
        if (player.ActionSlots == null)
        {
          continue;
        }
        foreach (var action in player.ActionSlots.Where(b => b?.CurrentTick == true))
        {
          var livedMonster = monster.Where(b => b.CurrentHp > 0).ToArray();
          await action.ActionSkill.Attack(player, livedMonster);
          var killedMonster = livedMonster.Where(b => b.CurrentHp <= 0).ToArray();
          foreach (var m in killedMonster)
          {
            var exp = ExpHelper.GetMonsterExp(m.Level);
            await owner.GainCurrency(exp);
            await Task.WhenAll(guests().Select(b => b.GainCurrency(exp)));
          }
          livedMonster = null;
          killedMonster = null;
        }

      }
      monster = null;
      return baseResult;
    }
  }
}
