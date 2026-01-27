namespace SkillCraft.Cms.Infrastructure.Entities;

internal class DoctrineFeatureEntity
{
  public DoctrineEntity? Doctrine { get; private set; }
  public int DoctrineId { get; private set; }
  public Guid DoctrineUid { get; private set; }

  public FeatureEntity? Feature { get; private set; }
  public int FeatureId { get; private set; }
  public Guid FeatureUid { get; private set; }

  public DoctrineFeatureEntity(DoctrineEntity doctrine, FeatureEntity feature)
  {
    Doctrine = doctrine;
    DoctrineId = doctrine.DoctrineId;
    DoctrineUid = doctrine.Id;

    Feature = feature;
    FeatureId = feature.FeatureId;
    FeatureUid = feature.Id;
  }

  private DoctrineFeatureEntity()
  {
  }

  public override bool Equals(object? obj) => obj is DoctrineFeatureEntity entity && entity.DoctrineId == DoctrineId && entity.FeatureId == FeatureId;
  public override int GetHashCode() => HashCode.Combine(DoctrineId, FeatureId);
  public override string ToString() => $"{GetType()} (DoctrineId={DoctrineId}, FeatureId={FeatureId})";
}
