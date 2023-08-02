﻿using IdleWithBlazor.Common.DTOs.Actors;
using IdleWithBlazor.Common.Interfaces.Actors;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Common.Helpers
{
  public static class ActorDTOHelper
  {
    public static T? ToDTO<T>(this IActor? input) where T : IActorDTO, new()
    {
      if (input == null)
      {
        return default(T?);
      }
      var dto = new T();
      SetDTO(input, dto);
      return dto;
    }

    public static void SetDTO(IActor input, IActorDTO? dto)
    {
      if (dto == null)
      {
        return;
      }
      dto.Name = input.Name;
      dto.Id = input.Id;
      foreach (var value in DTOSetMapper.Values)
      {
        value(input, dto);
      }

    }

    private static ConcurrentDictionary<Type, Action<IActor, IActorDTO>> DTOSetMapper = new ConcurrentDictionary<Type, Action<IActor, IActorDTO>>();
    public static void AddDTOMapper<K, T>(Action<K, T> mapperAdding) where T : IActorDTO, new() where K : IActor
    {
      DTOSetMapper.TryAdd(typeof(T), (actor, dto) =>
      {
        if (actor == null || dto == null)
        {
          return;
        }
        if (actor is K kActor && dto is T tDTO)
        {
          mapperAdding(kActor, tDTO);
        }
      });
    }

    public static void InitDTOMapper()
    {
      AddDTOMapper<ISprite, SpriteDTO>((input, dto) =>
      {
        dto.MaxHp = input.MaxHp;
        dto.CurrentHp = input.CurrentHp;
      });
      AddDTOMapper<ICharacter, CharacterDTO>((input, dto) =>
      {

      });
      AddDTOMapper<ICharacter, CharacterDetailDTO>((input, dto) =>
      {
        dto.GameSummary = input.Room.ToDTO<GameSummaryDTO>();
      });
      AddDTOMapper<IGameMap, GameMapDTO>((input, dto) =>
      {
        dto.Players = input.Players?.Select(b => b.ToDTO<PlayerDTO>());
        dto.Monsters = input.Monsters?.Select(b => b.ToDTO<MonsterDTO>());
      });
      AddDTOMapper<IGameRoom, GameRoomDTO>((input, dto) =>
      {
        dto.Owner = input.GameOwner?.ToDTO<CharacterDTO>();
        dto.Guests = input.Guests?.Select(x => x.ToDTO<CharacterDTO>());
        dto.GameMap = input.Map?.ToDTO<GameMapDTO>();
      });
      AddDTOMapper<IGameRoom, GameSummaryDTO>((input, dto) =>
      {
        dto.Guests = input.Guests?.Select(x => x.ToDTO<CharacterDTO>()) ?? null;
      });
    }



  }
}
