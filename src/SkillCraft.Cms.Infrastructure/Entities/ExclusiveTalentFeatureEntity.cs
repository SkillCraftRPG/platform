namespace SkillCraft.Cms.Infrastructure.Entities;

internal class ExclusiveTalentFeatureEntity
{
  public ExclusiveTalentEntity? ExclusiveTalent { get; private set; }
  public int ExclusiveTalentId { get; private set; }
  public Guid ExclusiveTalentUid { get; private set; }

  public FeatureEntity? Feature { get; private set; }
  public int FeatureId { get; private set; }
  public Guid FeatureUid { get; private set; }

  public ExclusiveTalentFeatureEntity(ExclusiveTalentEntity exclusiveTalent, FeatureEntity feature)
  {
    ExclusiveTalent = exclusiveTalent;
    ExclusiveTalentId = exclusiveTalent.ExclusiveTalentId;
    ExclusiveTalentUid = exclusiveTalent.Id;

    Feature = feature;
    FeatureId = feature.FeatureId;
    FeatureUid = feature.Id;
  }

  private ExclusiveTalentFeatureEntity()
  {
  }

  public override bool Equals(object? obj) => obj is ExclusiveTalentFeatureEntity entity && entity.ExclusiveTalentId == ExclusiveTalentId && entity.FeatureId == FeatureId;
  public override int GetHashCode() => HashCode.Combine(ExclusiveTalentId, FeatureId);
  public override string ToString() => $"{GetType()} (ExclusiveTalentId={ExclusiveTalentId}, FeatureId={FeatureId})";
}
