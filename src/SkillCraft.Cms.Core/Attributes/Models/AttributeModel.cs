using Krakenar.Contracts;

namespace SkillCraft.Cms.Core.Attributes.Models;

public class AttributeModel : Aggregate
{
  public string Slug { get; set; } = string.Empty;
  public string Name { get; set; } = string.Empty;

  public GameAttribute Value { get; set; }
  public AttributeCategory? Category { get; set; }

  public string? MetaDescription { get; set; }
  public string? Summary { get; set; }
  public string? HtmlContent { get; set; }

  // TODO(fpion): public List<StatisticModel> Statistics { get; set; } = [];
  // TODO(fpion): public List<SkillModel> Skills { get; set; } = [];

  public override string ToString() => $"{Name} | {base.ToString()}";
}
