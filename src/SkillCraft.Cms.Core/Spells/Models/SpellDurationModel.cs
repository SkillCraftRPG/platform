namespace SkillCraft.Cms.Core.Spells.Models;

public record SpellDurationModel
{
  public int Value { get; set; }
  public TimeUnit Unit { get; set; }
  public bool Concentration { get; set; }
}
