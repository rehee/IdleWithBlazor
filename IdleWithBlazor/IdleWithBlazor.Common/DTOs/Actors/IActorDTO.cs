using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Common.DTOs.Actors
{
  public interface IActorDTO
  {
    Guid Id { get; set; }
    string? Name { get; set; }
  }
}
