using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Common.DTOs.Actors
{
  public class SkillSlotDTO : ActorDTO
  {
    public string Name { get; set; }
    public int? Processing { get; set; }
    public bool IsPicked { get; set; }
  }
}
