using IdleWithBlazor.Common.Helpers;
using IdleWithBlazor.Common.Interfaces.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Test.Actors
{
  public class CharacterTest : TestBase
  {
    [Test]
    public async Task Character_Create_Game_HappyPath_Test()
    {
      var c1 = ActorHelper.New<ICharacter>();
      var room = await c1.CreateRoomAsync();

      Assert.That(c1.Id, Is.EqualTo(room.OwnerId));

      var c2 = ActorHelper.New<ICharacter>();
      await c2.JoinGameAsync(room);


      var c3 = ActorHelper.New<ICharacter>();
      await c3.JoinGameAsync(room);

      Assert.That(room.Guests.Count, Is.EqualTo(2));

      await room.CreateMapAsync();

      await room.Map.GenerateMobsAsync();

      Assert.That(room.Map.Players.Count(), Is.EqualTo(3));

      var c4 = ActorHelper.New<ICharacter>();
      await c4.JoinGameAsync(room);
      Assert.That(room.Map.Players.Count(), Is.EqualTo(4));

      await c4.LeaveGameAsync();
      Assert.That(room.Map.Players.Count(), Is.EqualTo(3));

      await c2.KickPlayerAsync(c3.Id);
      Assert.That(room.Map.Players.Count(), Is.EqualTo(3));
      await c1.KickPlayerAsync(c3.Id);
      Assert.That(room.Map.Players.Count(), Is.EqualTo(2));
      Assert.That(c3.Room, Is.Null);
      await c1.LeaveGameAsync();
      Assert.That(c1.Room, Is.Null);
      Assert.That(c2.Room, Is.Null);

    }

    [Test]
    public async Task Character_Player_Test()
    {
      var c1 = ActorHelper.New<ICharacter>();
      c1.Id = Guid.NewGuid();
      var player = c1.ThisPlayer;
      Assert.That(player.Id, Is.EqualTo(c1.Id));
      await c1.UpdatePlayerAsync();
    }

    [Test]
    public async Task Character_Create_Game_HappyPath_WithSkill_Test()
    {
      var c1 = ActorHelper.New<ICharacter>();
      c1.Init();
      
      var room = await c1.CreateRoomAsync();
      await room.CreateMapAsync();
      await room.Map.GenerateMobsAsync();
      for (var i = 0; i < 999; i++)
      {
        await room.OnTick(default(IServiceProvider));
      }
      c1.ActionSlots[0] = null;
      //Assert.That(room.Map.Monsters.FirstOrDefault().CurrentHp, Is.EqualTo(new BigInteger(10)));
      for (var i = 0; i < 999; i++)
      {
        await room.OnTick(default(IServiceProvider));
      }
      //Assert.That(room.Map.Monsters.FirstOrDefault().CurrentHp, Is.EqualTo(new BigInteger(10)));
    }
  }
}
