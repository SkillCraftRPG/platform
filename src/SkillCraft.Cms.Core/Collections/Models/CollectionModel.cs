using Krakenar.Contracts;
using SkillCraft.Cms.Core.Articles.Models;

namespace SkillCraft.Cms.Core.Collections.Models;

public class CollectionModel : Aggregate
{
  public string Slug { get; set; } = string.Empty;
  public string Name { get; set; } = string.Empty;

  public string? MetaDescription { get; set; }
  public string? HtmlContent { get; set; }

  public List<ArticleModel> Articles { get; set; } = [];

  public override string ToString() => $"{Name} | {base.ToString()}";
}
