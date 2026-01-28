using SkillCraft.Cms.Core.Talents.Models;

namespace SkillCraft.Cms.Core.Specializations.Models;

public record OptionsModel
{
  public List<TalentModel> Talents { get; set; } = [];
  public List<string> Other { get; set; } = [];
}
