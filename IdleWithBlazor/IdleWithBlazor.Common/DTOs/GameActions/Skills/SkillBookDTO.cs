using IdleWithBlazor.Common.DTOs.Actors;

namespace IdleWithBlazor.Common.DTOs.GameActions.Skills
{
  public class SkillBookDTO : ActorDTO
  {
    public SkillPoolDTO[]? SkillPools { get; set; }
    public Dictionary<int, SkillPickDTO?>? SkillSlots { get; set; }
  }
}
