using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Common.Interfaces
{
  public interface ISecondaryProperty
  {
    int? GoldFInd { get; set; }
    int? ExperienceAfterKill { get; set; }
    int? ExperiencePercentage { get; set; }
  }
}
