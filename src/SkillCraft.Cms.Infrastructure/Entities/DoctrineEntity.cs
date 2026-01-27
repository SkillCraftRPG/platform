using Krakenar.Core.Contents;
using Krakenar.Core.Contents.Events;
using Krakenar.EntityFrameworkCore.Relational.KrakenarDb;
using AggregateEntity = Krakenar.EntityFrameworkCore.Relational.Entities.Aggregate;

namespace SkillCraft.Cms.Infrastructure.Entities;

internal class DoctrineEntity : AggregateEntity
{
  public int DoctrineId { get; private set; }
  public Guid Id { get; private set; }

  public bool IsPublished { get; private set; }

  public string Key { get; set; } = string.Empty;
  public string KeyNormalized
  {
    get => Helper.Normalize(Key);
    private set { }
  }
  public string Name { get; set; } = string.Empty;

  public SpecializationEntity? Specialization { get; private set; }
  public int SpecializationId { get; private set; }
  public Guid SpecializationUid { get; private set; }

  public string? HtmlContent { get; set; }

  public List<DoctrineDiscountedTalentEntity> DiscountedTalents { get; private set; } = [];
  public List<DoctrineFeatureEntity> Features { get; private set; } = [];

  public DoctrineEntity(ContentLocalePublished @event) : base(@event)
  {
    Id = new ContentId(@event.StreamId).EntityId;
  }

  private DoctrineEntity() : base()
  {
  }

  public void AddDiscountedTalent(TalentEntity talent)
  {
    DiscountedTalents.Add(new DoctrineDiscountedTalentEntity(this, talent));
  }

  public void AddFeature(FeatureEntity feature)
  {
    Features.Add(new DoctrineFeatureEntity(this, feature));
  }

  public void Publish(ContentLocalePublished @event)
  {
    Update(@event);

    IsPublished = true;
  }

  public void SetSpecialization(SpecializationEntity specialization)
  {
    Specialization = specialization;
    SpecializationId = specialization.SpecializationId;
    SpecializationUid = specialization.Id;
  }

  public void Unpublish(ContentLocaleUnpublished @event)
  {
    Update(@event);

    IsPublished = false;
  }

  public override string ToString() => $"{Name} | {base.ToString()}";
}
