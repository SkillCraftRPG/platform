using Logitar.Data;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.RulesDb;

internal static class DoctrineFeatures
{
  public static readonly TableId Table = new(RulesContext.Schema, nameof(RulesContext.DoctrineFeatures), alias: null);

  public static readonly ColumnId DoctrineId = new(nameof(DoctrineFeatureEntity.DoctrineId), Table);
  public static readonly ColumnId DoctrineUid = new(nameof(DoctrineFeatureEntity.DoctrineUid), Table);
  public static readonly ColumnId FeatureId = new(nameof(DoctrineFeatureEntity.FeatureId), Table);
  public static readonly ColumnId FeatureUid = new(nameof(DoctrineFeatureEntity.FeatureUid), Table);
}
