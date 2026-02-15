namespace SkillCraft.Cms.Core.Quests.Models;

public class QuestModel
{
  public Guid Id { get; set; }

  public string Title { get; set; } = string.Empty;
  public QuestGroupModel? Group { get; set; }

  public int GrantedLevels { get; set; }
  public double ProgressRatio { get; set; }

  public string? HtmlContent { get; set; }
  public string? CompletedEntries { get; set; }
  public string? ActiveEntries { get; set; }

  public override bool Equals(object? obj) => obj is QuestModel quest && quest.Id == Id;
  public override int GetHashCode() => Id.GetHashCode();
  public override string ToString() => $"{Title} (Id={Id})";
}
