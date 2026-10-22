namespace SkillCraft.Cms.Infrastructure.Entities;

internal class SpecializationOriginEntity
{
  public SpecializationEntity? Specialization { get; private set; }
  public int SpecializationId { get; private set; }
  public Guid SpecializationUid { get; private set; }

  public SpecializationEntity? Origin { get; private set; }
  public int OriginId { get; private set; }
  public Guid OriginUid { get; private set; }

  public SpecializationOriginEntity(SpecializationEntity specialization, SpecializationEntity origin)
  {
    Specialization = specialization;
    SpecializationId = specialization.SpecializationId;
    SpecializationUid = specialization.Id;

    Origin = origin;
    OriginId = origin.SpecializationId;
    OriginUid = origin.Id;
  }

  private SpecializationOriginEntity()
  {
  }

  public override bool Equals(object? obj) => obj is SpecializationOriginEntity entity && entity.SpecializationId == SpecializationId && entity.OriginId == OriginId;
  public override int GetHashCode() => HashCode.Combine(SpecializationId, OriginId);
  public override string ToString() => $"{GetType()} (SpecializationId={SpecializationId}, OriginId={OriginId})";
}
