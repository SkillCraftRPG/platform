using Krakenar.Contracts;
using SkillCraft.Cms.Core.Collections.Models;

namespace SkillCraft.Cms.Core.Articles.Models;

public class ArticleModel : Aggregate
{
  public string Slug { get; set; } = string.Empty;
  public string Title { get; set; } = string.Empty;

  public string? MetaDescription { get; set; }
  public string? HtmlContent { get; set; }

  public CollectionModel Collection { get; set; } = new();
  public ArticleModel? Parent { get; set; }
  public List<ArticleModel> Children { get; set; } = [];

  public override string ToString() => $"{Title} | {base.ToString()}";
}
