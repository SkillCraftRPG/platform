using Krakenar.Core.Contents;
using Krakenar.Core.Contents.Events;
using Krakenar.EntityFrameworkCore.Relational.KrakenarDb;
using Logitar;
using Logitar.EventSourcing;
using SkillCraft.Cms.Core.Lineages;
using AggregateEntity = Krakenar.EntityFrameworkCore.Relational.Entities.Aggregate;

namespace SkillCraft.Cms.Infrastructure.Entities;

internal class LineageEntity : AggregateEntity
{
  public int LineageId { get; private set; }
  public Guid Id { get; private set; }

  public bool IsPublished { get; private set; }

  public string Slug { get; set; } = string.Empty;
  public string SlugNormalized
  {
    get => Helper.Normalize(Slug);
    private set { }
  }
  public string Name { get; set; } = string.Empty;

  public LineageEntity? Parent { get; private set; }
  public int? ParentId { get; private set; }
  public Guid? ParentUid { get; private set; }

  public int ExtraLanguages { get; set; }
  public string? LanguagesText { get; set; }

  public string? FamilyNames { get; set; }
  public string? FemaleNames { get; set; }
  public string? MaleNames { get; set; }
  public string? UnisexNames { get; set; }
  public string? CustomNames { get; set; }
  public string? NamesText { get; set; }

  public int Walk { get; set; }
  public int Climb { get; set; }
  public int Swim { get; set; }
  public int Fly { get; set; }
  public bool Hover { get; set; }
  public int Burrow { get; set; }

  public SizeCategory SizeCategory { get; set; }
  public string? SizeRoll { get; set; }

  public string? Malnutrition { get; set; }
  public string? Skinny { get; set; }
  public string? NormalWeight { get; set; }
  public string? Overweight { get; set; }
  public string? Obese { get; set; }

  public int Teenager { get; set; }
  public int Adult { get; set; }
  public int Mature { get; set; }
  public int Venerable { get; set; }

  public string? MetaDescription { get; set; }
  public string? Summary { get; set; }
  public string? HtmlContent { get; set; }

  public List<LineageEntity> Children { get; private set; } = [];
  public List<LineageFeatureEntity> Features { get; private set; } = [];
  public List<LineageLanguageEntity> Languages { get; private set; } = [];

  public LineageEntity(ContentLocalePublished @event) : base(@event)
  {
    Id = new ContentId(@event.StreamId).EntityId;
  }

  private LineageEntity() : base()
  {
  }

  public void AddFeature(FeatureEntity feature)
  {
    Features.Add(new LineageFeatureEntity(this, feature));
  }

  public void AddLanguage(LanguageEntity language)
  {
    Languages.Add(new LineageLanguageEntity(this, language));
  }

  public override IReadOnlyCollection<ActorId> GetActorIds()
  {
    HashSet<ActorId> actorIds = new(base.GetActorIds());
    if (Parent is not null)
    {
      actorIds.AddRange(Parent.GetActorIds());
    }
    foreach (LineageFeatureEntity feature in Features)
    {
      if (feature.Feature is not null)
      {
        actorIds.AddRange(feature.Feature.GetActorIds());
      }
    }
    foreach (LineageLanguageEntity language in Languages)
    {
      if (language.Language is not null)
      {
        actorIds.AddRange(language.Language.GetActorIds());
      }
    }
    return actorIds;
  }

  public void Publish(ContentLocalePublished @event)
  {
    Update(@event);

    IsPublished = true;
  }

  public void SetParent(LineageEntity? parent)
  {
    Parent = parent;
    ParentId = parent?.LineageId;
    ParentUid = parent?.Id;
  }

  public void Unpublish(ContentLocaleUnpublished @event)
  {
    Update(@event);

    IsPublished = false;
  }

  public override string ToString() => $"{Name} | {base.ToString()}";
}
