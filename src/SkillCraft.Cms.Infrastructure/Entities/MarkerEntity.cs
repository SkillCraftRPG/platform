using Krakenar.Core.Contents;
using Krakenar.Core.Contents.Events;
using Krakenar.EntityFrameworkCore.Relational.KrakenarDb;
using AggregateEntity = Krakenar.EntityFrameworkCore.Relational.Entities.Aggregate;

namespace SkillCraft.Cms.Infrastructure.Entities;

internal class MarkerEntity : AggregateEntity
{
  public int MarkerId { get; private set; }
  public Guid Id { get; private set; }

  public bool IsPublished { get; private set; }

  public string Key { get; set; } = string.Empty;
  public string KeyNormalized
  {
    get => Helper.Normalize(Key);
    private set { }
  }
  public string Title { get; set; } = string.Empty;

  public MapEntity? Map { get; private set; }
  public int MapId { get; private set; }
  public Guid MapUid { get; private set; }

  public int X { get; set; }
  public int Y { get; set; }

  public string? HtmlContent { get; set; }

  public MarkerEntity(ContentLocalePublished @event) : base(@event)
  {
    Id = new ContentId(@event.StreamId).EntityId;
  }

  private MarkerEntity() : base()
  {
  }

  public void Publish(ContentLocalePublished @event)
  {
    Update(@event);

    IsPublished = true;
  }

  public void SetMap(MapEntity map)
  {
    Map = map;
    MapId = map.MapId;
    MapUid = map.Id;
  }

  public void Unpublish(ContentLocaleUnpublished @event)
  {
    Update(@event);

    IsPublished = false;
  }

  public override string ToString() => $"{Title} | {base.ToString()}";
}
