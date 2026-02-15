using Logitar.Data;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.EncyclopediaDb;

internal static class Quests
{
  public static readonly TableId Table = new(EncyclopediaContext.Schema, nameof(EncyclopediaContext.Quests), alias: null);

  public static readonly ColumnId CreatedBy = new(nameof(QuestEntity.CreatedBy), Table);
  public static readonly ColumnId CreatedOn = new(nameof(QuestEntity.CreatedOn), Table);
  public static readonly ColumnId StreamId = new(nameof(QuestEntity.StreamId), Table);
  public static readonly ColumnId UpdatedBy = new(nameof(QuestEntity.UpdatedBy), Table);
  public static readonly ColumnId UpdatedOn = new(nameof(QuestEntity.UpdatedOn), Table);
  public static readonly ColumnId Version = new(nameof(QuestEntity.Version), Table);

  public static readonly ColumnId ActiveEntries = new(nameof(QuestEntity.ActiveEntries), Table);
  public static readonly ColumnId CompletedEntries = new(nameof(QuestEntity.CompletedEntries), Table);
  public static readonly ColumnId GrantedLevels = new(nameof(QuestEntity.GrantedLevels), Table);
  public static readonly ColumnId HtmlContent = new(nameof(QuestEntity.HtmlContent), Table);
  public static readonly ColumnId Id = new(nameof(QuestEntity.Id), Table);
  public static readonly ColumnId IsPublished = new(nameof(QuestEntity.IsPublished), Table);
  public static readonly ColumnId ProgressRatio = new(nameof(QuestEntity.ProgressRatio), Table);
  public static readonly ColumnId QuestGroupId = new(nameof(QuestEntity.QuestGroupId), Table);
  public static readonly ColumnId QuestGroupUid = new(nameof(QuestEntity.QuestGroupUid), Table);
  public static readonly ColumnId QuestId = new(nameof(QuestEntity.QuestId), Table);
  public static readonly ColumnId QuestLogId = new(nameof(QuestEntity.QuestLogId), Table);
  public static readonly ColumnId QuestLogUid = new(nameof(QuestEntity.QuestLogUid), Table);
  public static readonly ColumnId Title = new(nameof(QuestEntity.Title), Table);
}
