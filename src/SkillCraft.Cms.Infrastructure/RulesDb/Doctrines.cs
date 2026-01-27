using Logitar.Data;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.RulesDb;

internal static class Doctrines
{
  public static readonly TableId Table = new(RulesContext.Schema, nameof(RulesContext.Doctrines), alias: null);

  public static readonly ColumnId CreatedBy = new(nameof(DoctrineEntity.CreatedBy), Table);
  public static readonly ColumnId CreatedOn = new(nameof(DoctrineEntity.CreatedOn), Table);
  public static readonly ColumnId StreamId = new(nameof(DoctrineEntity.StreamId), Table);
  public static readonly ColumnId UpdatedBy = new(nameof(DoctrineEntity.UpdatedBy), Table);
  public static readonly ColumnId UpdatedOn = new(nameof(DoctrineEntity.UpdatedOn), Table);
  public static readonly ColumnId Version = new(nameof(DoctrineEntity.Version), Table);

  public static readonly ColumnId DoctrineId = new(nameof(DoctrineEntity.DoctrineId), Table);
  public static readonly ColumnId HtmlContent = new(nameof(DoctrineEntity.HtmlContent), Table);
  public static readonly ColumnId Id = new(nameof(DoctrineEntity.Id), Table);
  public static readonly ColumnId IsPublished = new(nameof(DoctrineEntity.IsPublished), Table);
  public static readonly ColumnId Key = new(nameof(DoctrineEntity.Key), Table);
  public static readonly ColumnId KeyNormalized = new(nameof(DoctrineEntity.KeyNormalized), Table);
  public static readonly ColumnId Name = new(nameof(DoctrineEntity.Name), Table);
  public static readonly ColumnId SpecializationId = new(nameof(DoctrineEntity.SpecializationId), Table);
  public static readonly ColumnId SpecializationUid = new(nameof(DoctrineEntity.SpecializationUid), Table);
}
