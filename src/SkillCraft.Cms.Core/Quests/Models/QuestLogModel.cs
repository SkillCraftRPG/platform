using Krakenar.Contracts;

namespace SkillCraft.Cms.Core.Quests.Models;

public class QuestLogModel : Aggregate
{
  public string Slug { get; set; } = string.Empty;
  public string Title { get; set; } = string.Empty;

  public string? MetaDescription { get; set; }
  public string? HtmlContent { get; set; }

  public List<QuestModel> Quests { get; set; } = [];

  public override string ToString() => $"{Title} | {base.ToString()}";
}
