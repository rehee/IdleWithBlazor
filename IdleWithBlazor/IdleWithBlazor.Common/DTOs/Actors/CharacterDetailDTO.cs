using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Common.DTOs.Actors
{
  public class CharacterDetailDTO : CharacterDTO
  {
    public GameSummaryDTO? GameSummary { get; set; }

  }
}
