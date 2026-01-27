using Logitar.Data;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.RulesDb;

internal static class ExclusiveTalentFeatures
{
  public static readonly TableId Table = new(RulesContext.Schema, nameof(RulesContext.ExclusiveTalentFeatures), alias: null);

  public static readonly ColumnId ExclusiveTalentId = new(nameof(ExclusiveTalentFeatureEntity.ExclusiveTalentId), Table);
  public static readonly ColumnId ExclusiveTalentUid = new(nameof(ExclusiveTalentFeatureEntity.ExclusiveTalentUid), Table);
  public static readonly ColumnId FeatureId = new(nameof(ExclusiveTalentFeatureEntity.FeatureId), Table);
  public static readonly ColumnId FeatureUid = new(nameof(ExclusiveTalentFeatureEntity.FeatureUid), Table);
}
