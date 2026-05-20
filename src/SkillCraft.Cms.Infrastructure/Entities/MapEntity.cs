using Krakenar.Core.Contents;
using Krakenar.Core.Contents.Events;
using Krakenar.EntityFrameworkCore.Relational.KrakenarDb;
using AggregateEntity = Krakenar.EntityFrameworkCore.Relational.Entities.Aggregate;

namespace SkillCraft.Cms.Infrastructure.Entities;

internal class MapEntity : AggregateEntity
{
  public int MapId { get; private set; }
  public Guid Id { get; private set; }

  public bool IsPublished { get; private set; }

  public string Key { get; set; } = string.Empty;
  public string KeyNormalized
  {
    get => Helper.Normalize(Key);
    private set { }
  }
  public string? Title { get; set; } = string.Empty;

  public int Width { get; set; }
  public int Height { get; set; }
  public string Source { get; set; } = string.Empty;

  public MapEntity(ContentLocalePublished @event) : base(@event)
  {
    Id = new ContentId(@event.StreamId).EntityId;
  }

  private MapEntity() : base()
  {
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

  public override string ToString() => $"{Title ?? Key} | {base.ToString()}";
}
