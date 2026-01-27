namespace SkillCraft.Cms.Infrastructure.Entities;

internal class ReservedTalentFeatureEntity
{
  public ReservedTalentEntity? ReservedTalent { get; private set; }
  public int ReservedTalentId { get; private set; }
  public Guid ReservedTalentUid { get; private set; }

  public FeatureEntity? Feature { get; private set; }
  public int FeatureId { get; private set; }
  public Guid FeatureUid { get; private set; }

  public ReservedTalentFeatureEntity(ReservedTalentEntity reservedTalent, FeatureEntity feature)
  {
    ReservedTalent = reservedTalent;
    ReservedTalentId = reservedTalent.ReservedTalentId;
    ReservedTalentUid = reservedTalent.Id;

    Feature = feature;
    FeatureId = feature.FeatureId;
    FeatureUid = feature.Id;
  }

  private ReservedTalentFeatureEntity()
  {
  }

  public override bool Equals(object? obj) => obj is ReservedTalentFeatureEntity entity && entity.ReservedTalentId == ReservedTalentId && entity.FeatureId == FeatureId;
  public override int GetHashCode() => HashCode.Combine(ReservedTalentId, FeatureId);
  public override string ToString() => $"{GetType()} (ReservedTalentId={ReservedTalentId}, FeatureId={FeatureId})";
}
