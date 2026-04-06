using Krakenar.Contracts;

namespace SkillCraft.Cms.Core.Lineages.Models;

public class SpeciesCategoryModel : Aggregate
{
  public string Key { get; set; } = string.Empty;
  public string Name { get; set; } = string.Empty;

  public int Order { get; set; }
  public int Columns { get; set; }

  public string? HtmlContent { get; set; }

  public List<SpeciesModel> Species { get; set; } = [];

  public override string ToString() => $"{Name} | {base.ToString()}";
}
