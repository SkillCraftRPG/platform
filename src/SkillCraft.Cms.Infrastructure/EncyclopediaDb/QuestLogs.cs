using Logitar.Data;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.EncyclopediaDb;

internal static class QuestLogs
{
  public static readonly TableId Table = new(EncyclopediaContext.Schema, nameof(EncyclopediaContext.QuestLogs), alias: null);

  public static readonly ColumnId CreatedBy = new(nameof(QuestLogEntity.CreatedBy), Table);
  public static readonly ColumnId CreatedOn = new(nameof(QuestLogEntity.CreatedOn), Table);
  public static readonly ColumnId StreamId = new(nameof(QuestLogEntity.StreamId), Table);
  public static readonly ColumnId UpdatedBy = new(nameof(QuestLogEntity.UpdatedBy), Table);
  public static readonly ColumnId UpdatedOn = new(nameof(QuestLogEntity.UpdatedOn), Table);
  public static readonly ColumnId Version = new(nameof(QuestLogEntity.Version), Table);

  public static readonly ColumnId HtmlContent = new(nameof(QuestLogEntity.HtmlContent), Table);
  public static readonly ColumnId Id = new(nameof(QuestLogEntity.Id), Table);
  public static readonly ColumnId IsPublished = new(nameof(QuestLogEntity.IsPublished), Table);
  public static readonly ColumnId MetaDescription = new(nameof(QuestLogEntity.MetaDescription), Table);
  public static readonly ColumnId QuestLogId = new(nameof(QuestLogEntity.QuestLogId), Table);
  public static readonly ColumnId Slug = new(nameof(QuestLogEntity.Slug), Table);
  public static readonly ColumnId SlugNormalized = new(nameof(QuestLogEntity.SlugNormalized), Table);
  public static readonly ColumnId Title = new(nameof(QuestLogEntity.Title), Table);
}
