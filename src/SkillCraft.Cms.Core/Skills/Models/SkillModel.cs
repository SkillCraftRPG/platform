using Krakenar.Contracts;
using SkillCraft.Cms.Core.Attributes.Models;
using SkillCraft.Cms.Core.Talents.Models;

namespace SkillCraft.Cms.Core.Skills.Models;

public class SkillModel : Aggregate
{
  public string Slug { get; set; } = string.Empty;
  public string Name { get; set; } = string.Empty;

  public GameSkill Value { get; set; }
  public AttributeModel? Attribute { get; set; }

  public string? MetaDescription { get; set; }
  public string? Summary { get; set; }
  public string? HtmlContent { get; set; }

  public List<TalentModel> Talents { get; set; } = [];

  public override string ToString() => $"{Name} | {base.ToString()}";
}
