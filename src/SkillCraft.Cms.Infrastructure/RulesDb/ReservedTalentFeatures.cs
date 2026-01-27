using Logitar.Data;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.RulesDb;

internal static class ReservedTalentFeatures
{
  public static readonly TableId Table = new(RulesContext.Schema, nameof(RulesContext.ReservedTalentFeatures), alias: null);

  public static readonly ColumnId FeatureId = new(nameof(ReservedTalentFeatureEntity.FeatureId), Table);
  public static readonly ColumnId FeatureUid = new(nameof(ReservedTalentFeatureEntity.FeatureUid), Table);
  public static readonly ColumnId ReservedTalentId = new(nameof(ReservedTalentFeatureEntity.ReservedTalentId), Table);
  public static readonly ColumnId ReservedTalentUid = new(nameof(ReservedTalentFeatureEntity.ReservedTalentUid), Table);
}
