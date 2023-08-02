﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Common.DTOs.Actors
{
  public class ActorDTO : IActorDTO
  {
    public Guid Id { get; set; }
    public string? Name { get; set; }
  }
}
