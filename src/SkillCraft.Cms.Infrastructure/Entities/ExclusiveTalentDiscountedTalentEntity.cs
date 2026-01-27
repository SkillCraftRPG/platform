namespace SkillCraft.Cms.Infrastructure.Entities;

internal class ExclusiveTalentDiscountedTalentEntity
{
  public ExclusiveTalentEntity? ExclusiveTalent { get; private set; }
  public int ExclusiveTalentId { get; private set; }
  public Guid ExclusiveTalentUid { get; private set; }

  public TalentEntity? Talent { get; private set; }
  public int TalentId { get; private set; }
  public Guid TalentUid { get; private set; }

  public ExclusiveTalentDiscountedTalentEntity(ExclusiveTalentEntity exclusiveTalent, TalentEntity talent)
  {
    ExclusiveTalent = exclusiveTalent;
    ExclusiveTalentId = exclusiveTalent.ExclusiveTalentId;
    ExclusiveTalentUid = exclusiveTalent.Id;

    Talent = talent;
    TalentId = talent.TalentId;
    TalentUid = talent.Id;
  }

  private ExclusiveTalentDiscountedTalentEntity()
  {
  }

  public override bool Equals(object? obj) => obj is ExclusiveTalentDiscountedTalentEntity entity && entity.ExclusiveTalentId == ExclusiveTalentId && entity.TalentId == TalentId;
  public override int GetHashCode() => HashCode.Combine(ExclusiveTalentId, TalentId);
  public override string ToString() => $"{GetType()} (ExclusiveTalentId={ExclusiveTalentId}, TalentId={TalentId})";
}
