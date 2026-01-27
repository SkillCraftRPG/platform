using Logitar.Data;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.RulesDb;

internal static class ReservedTalents
{
  public static readonly TableId Table = new(RulesContext.Schema, nameof(RulesContext.ReservedTalents), alias: null);

  public static readonly ColumnId CreatedBy = new(nameof(ReservedTalentEntity.CreatedBy), Table);
  public static readonly ColumnId CreatedOn = new(nameof(ReservedTalentEntity.CreatedOn), Table);
  public static readonly ColumnId StreamId = new(nameof(ReservedTalentEntity.StreamId), Table);
  public static readonly ColumnId UpdatedBy = new(nameof(ReservedTalentEntity.UpdatedBy), Table);
  public static readonly ColumnId UpdatedOn = new(nameof(ReservedTalentEntity.UpdatedOn), Table);
  public static readonly ColumnId Version = new(nameof(ReservedTalentEntity.Version), Table);

  public static readonly ColumnId HtmlContent = new(nameof(ReservedTalentEntity.HtmlContent), Table);
  public static readonly ColumnId Id = new(nameof(ReservedTalentEntity.Id), Table);
  public static readonly ColumnId IsPublished = new(nameof(ReservedTalentEntity.IsPublished), Table);
  public static readonly ColumnId Key = new(nameof(ReservedTalentEntity.Key), Table);
  public static readonly ColumnId KeyNormalized = new(nameof(ReservedTalentEntity.KeyNormalized), Table);
  public static readonly ColumnId Name = new(nameof(ReservedTalentEntity.Name), Table);
  public static readonly ColumnId ReservedTalentId = new(nameof(ReservedTalentEntity.ReservedTalentId), Table);
  public static readonly ColumnId SpecializationId = new(nameof(ReservedTalentEntity.SpecializationId), Table);
  public static readonly ColumnId SpecializationUid = new(nameof(ReservedTalentEntity.SpecializationUid), Table);
}
