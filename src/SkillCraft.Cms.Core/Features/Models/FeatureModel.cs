using Krakenar.Contracts;

namespace SkillCraft.Cms.Core.Features.Models;

public class FeatureModel : Aggregate
{
  public string Key { get; set; } = string.Empty;
  public string Name { get; set; } = string.Empty;

  public string? HtmlContent { get; set; }

  public override string ToString() => $"{Name} | {base.ToString()}";
}
