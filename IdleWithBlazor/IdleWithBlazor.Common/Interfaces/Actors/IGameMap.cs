﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IdleWithBlazor.Common.Interfaces.Actors
{
  public interface IGameMap : IActor
  {
    Task<bool> InitAsync(ICharacters? owner, ConcurrentDictionary<Guid, ICharacters>? guests);
    Task<bool> GenerateMobsAsync();
    Task<bool> CloseMapAsync();

    IEnumerable<IPlayer?> Players { get; }
    IEnumerable<IMonster?> Monsters { get; }
  }
}