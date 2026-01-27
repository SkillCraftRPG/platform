using Logitar.Data;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.RulesDb;

internal static class ExclusiveTalentDiscountedTalents
{
  public static readonly TableId Table = new(RulesContext.Schema, nameof(RulesContext.ExclusiveTalentDiscountedTalents), alias: null);

  public static readonly ColumnId ExclusiveTalentId = new(nameof(ExclusiveTalentDiscountedTalentEntity.ExclusiveTalentId), Table);
  public static readonly ColumnId ExclusiveTalentUid = new(nameof(ExclusiveTalentDiscountedTalentEntity.ExclusiveTalentUid), Table);
  public static readonly ColumnId TalentId = new(nameof(ExclusiveTalentDiscountedTalentEntity.TalentId), Table);
  public static readonly ColumnId TalentUid = new(nameof(ExclusiveTalentDiscountedTalentEntity.TalentUid), Table);
}
