using Krakenar.Contracts;
using SkillCraft.Cms.Core.Attributes.Models;

namespace SkillCraft.Cms.Core.Statistics.Models;

public class StatisticModel : Aggregate
{
  public string Slug { get; set; } = string.Empty;
  public string Name { get; set; } = string.Empty;

  public GameStatistic Value { get; set; }
  public AttributeModel Attribute { get; set; } = new();

  public string? MetaDescription { get; set; }
  public string? Summary { get; set; }
  public string? HtmlContent { get; set; }

  public override string ToString() => $"{Name} | {base.ToString()}";
}
