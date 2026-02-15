using Logitar.Data;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.EncyclopediaDb;

internal static class QuestGroups
{
  public static readonly TableId Table = new(EncyclopediaContext.Schema, nameof(EncyclopediaContext.QuestGroups), alias: null);

  public static readonly ColumnId CreatedBy = new(nameof(QuestGroupEntity.CreatedBy), Table);
  public static readonly ColumnId CreatedOn = new(nameof(QuestGroupEntity.CreatedOn), Table);
  public static readonly ColumnId StreamId = new(nameof(QuestGroupEntity.StreamId), Table);
  public static readonly ColumnId UpdatedBy = new(nameof(QuestGroupEntity.UpdatedBy), Table);
  public static readonly ColumnId UpdatedOn = new(nameof(QuestGroupEntity.UpdatedOn), Table);
  public static readonly ColumnId Version = new(nameof(QuestGroupEntity.Version), Table);

  public static readonly ColumnId Id = new(nameof(QuestGroupEntity.Id), Table);
  public static readonly ColumnId IsPublished = new(nameof(QuestGroupEntity.IsPublished), Table);
  public static readonly ColumnId Name = new(nameof(QuestGroupEntity.Name), Table);
  public static readonly ColumnId QuestGroupId = new(nameof(QuestGroupEntity.QuestGroupId), Table);
}
