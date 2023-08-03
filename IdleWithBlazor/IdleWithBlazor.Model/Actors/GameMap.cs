using IdleWithBlazor.Common.Helpers;
using IdleWithBlazor.Common.Interfaces.Actors;
using System.Collections.Concurrent;
using System.Text.Json.Serialization;

namespace IdleWithBlazor.Model.Actors
{
  public class GameMap : Actor, IGameMap
  {
    public override Type TypeDiscriminator => typeof(GameMap);
    public override IEnumerable<IActor> Children
    {
      get
      {
        var players = Players.Select(b =>
        {
          if (b is IActor a)
          {
            return (true, a);
          }
          return (false, default(IActor));
        }).Where(b => b.Item1).Select(b => b.Item2);
        var mobs = Monsters.Select(b =>
        {
          if (b is IActor a)
          {
            return (true, a);
          }
          return (false, default(IActor));
        }).Where(b => b.Item1).Select(b => b.Item2);

        return players.Concat(mobs);
      }
      set => base.Children = value;
    }
    public IEnumerable<IPlayer?> Players => (new IPlayer?[] { owner?.ThisPlayer }).Concat(guests?.Values.Select(b => b.ThisPlayer) ?? Enumerable.Empty<IPlayer?>());
    public IEnumerable<IMonster?> Monsters => monsters ?? Enumerable.Empty<IMonster?>();

    private List<IMonster> monsters { get; set; }

    private ICharacter? owner { get; set; }
    private ConcurrentDictionary<Guid, ICharacter>? guests { get; set; }

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
      }
      monsters.Clear();
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
      return Task.FromResult(true);
    }

    public Task<bool> InitAsync(ICharacter? owner, ConcurrentDictionary<Guid, ICharacter>? guests)
    {
      this.owner = owner;
      this.guests = guests;
      MonsterRespawnRate = 50;
      MonsterRespawn = MonsterRespawnRate;
      FirstRespawn = true;
      return Task.FromResult(true);
    }
    private bool FirstRespawn { get; set; }
    public int MonsterRespawnRate { get; set; }
    public int MonsterRespawn { get; set; }
    public override async Task<bool> OnTick(IServiceProvider sp)
    {
      var baseResult = await base.OnTick(sp);
      var monster = Monsters;
      foreach (var player in Players.Where(b => b != null).ToArray())
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
            await Task.WhenAll(guests.Values.Select(b => b.GainCurrency(exp)));
          }
          livedMonster = null;
          killedMonster = null;
        }

      }
      if (monster.All(b => b.CurrentHp <= 0))
      {
        await GenerateMobsAsync();
      }
      return baseResult;
    }
  }
}
