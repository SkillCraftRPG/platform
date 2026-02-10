using Krakenar.Contracts;
using SkillCraft.Cms.Core.Features.Models;

namespace SkillCraft.Cms.Core.Lineages.Models;

public abstract class LineageBase : Aggregate
{
  public string Slug { get; set; } = string.Empty;
  public string Name { get; set; } = string.Empty;

  public LanguagesModel Languages { get; set; } = new();
  public NamesModel Names { get; set; } = new();

  public SpeedsModel Speeds { get; set; } = new();
  public SizeModel Size { get; set; } = new();
  public WeightModel Weight { get; set; } = new();
  public AgeModel Age { get; set; } = new();

  public string? MetaDescription { get; set; }
  public string? Summary { get; set; }
  public string? HtmlContent { get; set; }

  public List<FeatureModel> Features { get; set; } = [];

  public override string ToString() => $"{Name} | {base.ToString()}";
}
