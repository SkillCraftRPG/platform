using Krakenar.Core.Contents;
using Krakenar.Core.Contents.Events;
using Krakenar.EntityFrameworkCore.Relational.KrakenarDb;
using Logitar;
using Logitar.EventSourcing;
using AggregateEntity = Krakenar.EntityFrameworkCore.Relational.Entities.Aggregate;

namespace SkillCraft.Cms.Infrastructure.Entities;

internal class QuestLogEntity : AggregateEntity
{
  public int QuestLogId { get; private set; }
  public Guid Id { get; private set; }

  public bool IsPublished { get; private set; }

  public string Slug { get; set; } = string.Empty;
  public string SlugNormalized
  {
    get => Helper.Normalize(Slug);
    private set { }
  }
  public string Title { get; set; } = string.Empty;

  public string? MetaDescription { get; set; }
  public string? HtmlContent { get; set; }

  public List<QuestEntity> Quests { get; private set; } = [];

  public QuestLogEntity(ContentLocalePublished @event) : base(@event)
  {
    Id = new ContentId(@event.StreamId).EntityId;
  }

  private QuestLogEntity() : base()
  {
  }

  public override IReadOnlyCollection<ActorId> GetActorIds()
  {
    HashSet<ActorId> actorIds = new(base.GetActorIds());
    foreach (QuestEntity quest in Quests)
    {
      actorIds.AddRange(quest.GetActorIds());
    }
    return actorIds;
  }

  public void Publish(ContentLocalePublished @event)
  {
    Update(@event);

    IsPublished = true;
  }

  public void Unpublish(ContentLocaleUnpublished @event)
  {
    Update(@event);

    IsPublished = false;
  }

  public override string ToString() => $"{Title} | {base.ToString()}";
}
