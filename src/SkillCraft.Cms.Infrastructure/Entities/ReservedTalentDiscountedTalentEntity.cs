namespace SkillCraft.Cms.Infrastructure.Entities;

internal class ReservedTalentDiscountedTalentEntity
{
  public ReservedTalentEntity? ReservedTalent { get; private set; }
  public int ReservedTalentId { get; private set; }
  public Guid ReservedTalentUid { get; private set; }

  public TalentEntity? Talent { get; private set; }
  public int TalentId { get; private set; }
  public Guid TalentUid { get; private set; }

  public ReservedTalentDiscountedTalentEntity(ReservedTalentEntity reservedTalent, TalentEntity talent)
  {
    ReservedTalent = reservedTalent;
    ReservedTalentId = reservedTalent.ReservedTalentId;
    ReservedTalentUid = reservedTalent.Id;

    Talent = talent;
    TalentId = talent.TalentId;
    TalentUid = talent.Id;
  }

  private ReservedTalentDiscountedTalentEntity()
  {
  }

  public override bool Equals(object? obj) => obj is ReservedTalentDiscountedTalentEntity entity && entity.ReservedTalentId == ReservedTalentId && entity.TalentId == TalentId;
  public override int GetHashCode() => HashCode.Combine(ReservedTalentId, TalentId);
  public override string ToString() => $"{GetType()} (ReservedTalentId={ReservedTalentId}, TalentId={TalentId})";
}
