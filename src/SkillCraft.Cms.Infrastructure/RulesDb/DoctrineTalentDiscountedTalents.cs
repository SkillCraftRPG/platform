using Logitar.Data;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.RulesDb;

internal static class DoctrineTalentDiscountedTalents
{
  public static readonly TableId Table = new(RulesContext.Schema, nameof(RulesContext.DoctrineTalentDiscountedTalents), alias: null);

  public static readonly ColumnId DoctrineId = new(nameof(DoctrineDiscountedTalentEntity.DoctrineId), Table);
  public static readonly ColumnId DoctrineUid = new(nameof(DoctrineDiscountedTalentEntity.DoctrineUid), Table);
  public static readonly ColumnId TalentId = new(nameof(DoctrineDiscountedTalentEntity.TalentId), Table);
  public static readonly ColumnId TalentUid = new(nameof(DoctrineDiscountedTalentEntity.TalentUid), Table);
}
