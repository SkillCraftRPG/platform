namespace SkillCraft.Cms.Infrastructure.Entities;

internal class DoctrineDiscountedTalentEntity
{
  public DoctrineEntity? Doctrine { get; private set; }
  public int DoctrineId { get; private set; }
  public Guid DoctrineUid { get; private set; }

  public TalentEntity? Talent { get; private set; }
  public int TalentId { get; private set; }
  public Guid TalentUid { get; private set; }

  public DoctrineDiscountedTalentEntity(DoctrineEntity doctrine, TalentEntity talent)
  {
    Doctrine = doctrine;
    DoctrineId = doctrine.DoctrineId;
    DoctrineUid = doctrine.Id;

    Talent = talent;
    TalentId = talent.TalentId;
    TalentUid = talent.Id;
  }

  private DoctrineDiscountedTalentEntity()
  {
  }

  public override bool Equals(object? obj) => obj is DoctrineDiscountedTalentEntity entity && entity.DoctrineId == DoctrineId && entity.TalentId == TalentId;
  public override int GetHashCode() => HashCode.Combine(DoctrineId, TalentId);
  public override string ToString() => $"{GetType()} (DoctrineId={DoctrineId}, TalentId={TalentId})";
}
