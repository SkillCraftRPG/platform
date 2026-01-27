using Logitar.Data;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.RulesDb;

internal static class ExclusivedTalents
{
  public static readonly TableId Table = new(RulesContext.Schema, nameof(RulesContext.ExclusivedTalents), alias: null);

  public static readonly ColumnId CreatedBy = new(nameof(ExclusiveTalentEntity.CreatedBy), Table);
  public static readonly ColumnId CreatedOn = new(nameof(ExclusiveTalentEntity.CreatedOn), Table);
  public static readonly ColumnId StreamId = new(nameof(ExclusiveTalentEntity.StreamId), Table);
  public static readonly ColumnId UpdatedBy = new(nameof(ExclusiveTalentEntity.UpdatedBy), Table);
  public static readonly ColumnId UpdatedOn = new(nameof(ExclusiveTalentEntity.UpdatedOn), Table);
  public static readonly ColumnId Version = new(nameof(ExclusiveTalentEntity.Version), Table);

  public static readonly ColumnId ExclusiveTalentId = new(nameof(ExclusiveTalentEntity.ExclusiveTalentId), Table);
  public static readonly ColumnId HtmlContent = new(nameof(ExclusiveTalentEntity.HtmlContent), Table);
  public static readonly ColumnId Id = new(nameof(ExclusiveTalentEntity.Id), Table);
  public static readonly ColumnId IsPublished = new(nameof(ExclusiveTalentEntity.IsPublished), Table);
  public static readonly ColumnId Key = new(nameof(ExclusiveTalentEntity.Key), Table);
  public static readonly ColumnId KeyNormalized = new(nameof(ExclusiveTalentEntity.KeyNormalized), Table);
  public static readonly ColumnId Name = new(nameof(ExclusiveTalentEntity.Name), Table);
  public static readonly ColumnId SpecializationId = new(nameof(ExclusiveTalentEntity.SpecializationId), Table);
  public static readonly ColumnId SpecializationUid = new(nameof(ExclusiveTalentEntity.SpecializationUid), Table);
}
