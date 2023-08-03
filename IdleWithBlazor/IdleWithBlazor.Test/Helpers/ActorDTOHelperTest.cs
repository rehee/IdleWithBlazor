using IdleWithBlazor.Common.DTOs.Actors;
using IdleWithBlazor.Common.Helpers;
using IdleWithBlazor.Common.Interfaces.Actors;
using IdleWithBlazor.Model.Actors;
using IdleWithBlazor.Model.Characters;
using IdleWithBlazor.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Test.Helpers
{
  public class ActorDTOHelperTest : TestBase
  {
    [TestCaseSource(nameof(ActorDTOHelper_ToDTO_Test_Data))]
    public void ActorDTOHelper_ToDTO_Test(int index, IActor input, IActorDTO dto, IActorDTO actorDTO)
    {
      ActorDTOHelper.SetDTO(input, dto);
      var json1 = JsonHelper.ToJson(dto);
      var json2 = JsonHelper.ToJson(actorDTO);

      Assert.That(json1, Is.EqualTo(json2));
    }

    private static IEnumerable<TestCaseData> ActorDTOHelper_ToDTO_Test_Data()
    {
      ModelHelper.InitModel();
      yield return new TestCaseData(
        1,
        new Character
        {
          Id = Guid.Empty,
          Name = "111"
        },
        new CharacterDTO
        {

        },
        new CharacterDTO
        {
          Id = Guid.Empty,
          Name = "111"
        });
      yield return new TestCaseData(
        2,
        new Character
        {
          Id = Guid.Empty,
          Name = "112"
        },
        new CharacterDetailDTO
        {

        },
        new CharacterDetailDTO
        {
          Id = Guid.Empty,
          Name = "112"
        });
      yield return new TestCaseData(
        3,
        new Character
        {
          Id = Guid.Empty,
          Name = "112",
          Room = new GameRoom
          {

          }
        },
        new CharacterDetailDTO
        {

        },
        new CharacterDetailDTO
        {
          Id = Guid.Empty,
          Name = "112",
          GameSummary = new GameSummaryDTO
          {
            Guests = Enumerable.Empty<CharacterDTO>()
          }
        }); ;
      var user4 = new Character
      {
        Id = Guid.Empty,
        Name = "112",
      };
      Task.WaitAll(user4.CreateRoomAsync());
      Task.WaitAll(user4.Room.JoinGameAsync(new Character { Id = Guid.Empty, Name = "555" }));
      user4.Room.Id = Guid.Empty;
      user4.Room.Name = "123";

      yield return new TestCaseData(
        4,
        user4,
        new CharacterDetailDTO
        {

        },
        new CharacterDetailDTO
        {
          Id = Guid.Empty,
          Name = "112",
          GameSummary = new GameSummaryDTO
          {
            Id = Guid.Empty,
            Name = "123",
            Guests = new CharacterDTO[]
            {
              new CharacterDTO { Id = Guid.Empty,Name="555"}
            }
          }
        });
      var user5 = new Character
      {
        Id = Guid.Empty,
        Name = "112",
      };
      Task.WaitAll(user5.CreateRoomAsync());
      Task.WaitAll(user5.Room.JoinGameAsync(new Character { Id = Guid.Empty, Name = "555" }));
      user5.Room.Id = Guid.Empty;
      user5.Room.Name = "123";
      yield return new TestCaseData(
        5,
        user5.Room,
        new GameRoomDTO
        {

        },
        new GameRoomDTO
        {
          Id = Guid.Empty,
          Name = "123",
          Owner = new CharacterDTO { Id = Guid.Empty, Name = "112", },
          Guests = new CharacterDTO[] { new CharacterDTO { Id = Guid.Empty, Name = "555" } }

        });
    }
  }
}
