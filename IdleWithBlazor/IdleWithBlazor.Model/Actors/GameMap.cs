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
    
    private ICharacters? owner { get; set; }
    private ConcurrentDictionary<Guid, ICharacters>? guests { get; set; }

    public async Task<bool> CloseMapAsync()
    {
      this.owner = null;
      this.guests = null;
      await DisposeAsync();
      return true;
    }

    public Task<bool> GenerateMobsAsync()
    {
      lock (this)
      {
        if (monsters == null)
        {
          monsters = new List<IMonster>();
        }
        monsters.Clear();
        var m = ActorHelper.New<IMonster>();
        m.CurrentHp = 100;
        if (m != null)
        {
          monsters.Add(m);
        }
      }
      return Task.FromResult(true);
    }

    public Task<bool> InitAsync(ICharacters? owner, ConcurrentDictionary<Guid, ICharacters>? guests)
    {
      this.owner = owner;
      this.guests = guests;
      return Task.FromResult(true);
    }

    public override async Task<bool> OnTick(IServiceProvider sp)
    {
      var baseResult = await base.OnTick(sp);
      var monster = Monsters;
      foreach (var player in Players.Where(b => b != null).ToArray())
      {
        if (player.ActionSlots == null || player.ActionSlots.ActionSkill == null || player.ActionSlots.CurrentTick != true)
        {
          continue;
        }
        await player.ActionSlots.ActionSkill.Attack(player, monster.Where(b => b != null));
      }
      return baseResult;
    }
  }
}
