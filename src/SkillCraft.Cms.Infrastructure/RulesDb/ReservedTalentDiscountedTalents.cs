using Logitar.Data;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.RulesDb;

internal static class ReservedTalentDiscountedTalents
{
  public static readonly TableId Table = new(RulesContext.Schema, nameof(RulesContext.ReservedTalentDiscountedTalents), alias: null);

  public static readonly ColumnId ReservedTalentId = new(nameof(ReservedTalentDiscountedTalentEntity.ReservedTalentId), Table);
  public static readonly ColumnId ReservedTalentUid = new(nameof(ReservedTalentDiscountedTalentEntity.ReservedTalentUid), Table);
  public static readonly ColumnId TalentId = new(nameof(ReservedTalentDiscountedTalentEntity.TalentId), Table);
  public static readonly ColumnId TalentUid = new(nameof(ReservedTalentDiscountedTalentEntity.TalentUid), Table);
}
