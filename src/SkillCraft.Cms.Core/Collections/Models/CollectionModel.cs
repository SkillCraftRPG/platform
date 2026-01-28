using Krakenar.Contracts;
using SkillCraft.Cms.Core.Articles.Models;

namespace SkillCraft.Cms.Core.Collections.Models;

public class CollectionModel : Aggregate
{
  public string Key { get; set; } = string.Empty;
  public string? Name { get; set; }

  public List<ArticleModel> Articles { get; set; } = [];

  public override string ToString() => $"{Name ?? Key} | {base.ToString()}";
}
