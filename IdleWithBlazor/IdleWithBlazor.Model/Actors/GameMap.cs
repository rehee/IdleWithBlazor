using IdleWithBlazor.Common.Helpers;
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
        foreach (var mob in monsters.Where(b => b.CurrentHp > 0))
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
    public int MapLevel { get; set; }
    public int PackSize { get; set; }
    private List<IMonster> monsters { get; set; }

    private ICharacter? owner { get; set; }
    private Func<IEnumerable<ICharacter>> guests { get; set; }

    public async Task<bool> CloseMapAsync()
    {
      this.owner = null;
      this.guests = null;
      if (monsters?.Any() == true)
      {
        foreach (var m in monsters)
        {
          m.Dispose();
        }
        monsters.Clear();
      }
      monsters = null;
      await DisposeAsync();
      return true;
    }

    public async Task<bool> GenerateMobsAsync()
    {
      if (!FirstRespawn && MonsterRespawn > 0)
      {
        MonsterRespawn--;
        return false;
      }
      if (!FirstRespawn)
      {
        MapLevel++;
      }
      FirstRespawn = false;
      MonsterRespawn = MonsterRespawnRate;
      if (monsters == null)
      {
        monsters = new List<IMonster>();
      }
      else
      {
        foreach (var m in monsters)
        {
          m.Dispose();
        }
        monsters.Clear();
      }
      for (var i = 0; i < PackSize; i++)
      {
        var m = ActorHelper.New<IMonster>();
        m.Name = "小野怪";
        m.MaxHp = 10 + 10 * (MapLevel - 1);
        m.MinAttack = 1 + 10 * (MapLevel - 1);
        m.CurrentHp = m.MaxHp;
        m.Level = MapLevel;
        m.Init(null, ActorHelper.ActionSkillPool.FirstOrDefault());
        if (m != null)
        {
          monsters.Add(m);
        }
      }
      await owner.GainCurrency(0);
      foreach (var g in guests())
      {
        await g.GainCurrency(0);
      }
      return true;
    }

    public override void Init(IActor? parent, params object[] setInfo)
    {
      base.Init(parent, setInfo);
      if (parent != null && parent is IGameRoom room)
      {
        this.owner = room.GameOwner;
        this.guests = () => room.Guests();
        MapLevel = 1;
        PackSize = 5;
        MonsterRespawnRate = TickHelper.GetColdDownTick(null, 5);
        MonsterRespawn = MonsterRespawnRate;
        FirstRespawn = true;
      }
    }

    private bool FirstRespawn { get; set; }
    public int MonsterRespawnRate { get; set; }
    public int MonsterRespawn { get; set; }
    public override async Task<bool> OnTick(IServiceProvider sp)
    {
      if (owner.ThisPlayer.CurrentHp <= 0)
      {
        MapLevel = MapLevel - 2;
        if (MapLevel <= 0)
        {
          MapLevel = 1;
        }
        return await GenerateMobsAsync();
      }
      var monster = Monsters.ToList();
      var players = Players();

      if (monster.All(b => b.CurrentHp <= 0))
      {
        monster = null;
        return await GenerateMobsAsync();
      }

      var baseResult = await base.OnTick(sp);

      foreach (var player in players.Where(p => p.ActionSlots?.Any() == true && p.CurrentHp > 0))
      {
        foreach (var action in player.ActionSlots.Where(b => b?.CurrentTick == true))
        {
          var livedMonster = monster.Where(b => b.CurrentHp > 0).ToArray();
          await action.ActionSkill.Attack(player, livedMonster);
          var killedMonster = livedMonster.Where(b => b.CurrentHp <= 0).ToArray();
          foreach (var m in killedMonster)
          {
            var exp = ExpHelper.GetMonsterExp(m.Level);

            var itemDrop = await TemplateHelper.GetRamdonDrop(this, m);
            if (itemDrop != null)
            {
              if (!await owner?.PickItemAsync(itemDrop))
              {
                itemDrop.Dispose();
                itemDrop = null;
              }
            }
            await owner.GainCurrency(exp);
            await Task.WhenAll(guests().Select(b => b.GainCurrency(exp)));
          }
          livedMonster = null;
          killedMonster = null;
        }

      }
      foreach (var mob in monster.Where(b => b.ActionSlots?.Any() == true && b.CurrentHp > 0))
      {
        foreach (var action in mob.ActionSlots.Where(b => b?.CurrentTick == true))
        {
          await action.ActionSkill.Attack(mob, players.Where(b => b.CurrentHp > 0));
        }
      }
      players = null;
      monster = null;
      return baseResult;
    }
  }
}
