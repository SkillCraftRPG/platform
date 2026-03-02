using Krakenar.Core.Contents;
using Krakenar.Core.Contents.Events;
using Krakenar.EntityFrameworkCore.Relational.KrakenarDb;
using AggregateEntity = Krakenar.EntityFrameworkCore.Relational.Entities.Aggregate;

namespace SkillCraft.Cms.Infrastructure.Entities;

internal class SpellCategoryEntity : AggregateEntity
{
  public int SpellCategoryId { get; private set; }
  public Guid Id { get; private set; }

  public bool IsPublished { get; private set; }

  public string Key { get; set; } = string.Empty;
  public string KeyNormalized
  {
    get => Helper.Normalize(Key);
    private set { }
  }
  public string? Name { get; set; }

  public SpellCategoryEntity? Parent { get; private set; }
  public int? ParentId { get; private set; }
  public Guid? ParentUid { get; private set; }
  public List<SpellCategoryEntity> Children { get; private set; } = [];

  public SpellCategoryEntity(ContentLocalePublished @event) : base(@event)
  {
    Id = new ContentId(@event.StreamId).EntityId;
  }

  private SpellCategoryEntity() : base()
  {
  }

  public void Publish(ContentLocalePublished @event)
  {
    Update(@event);

    IsPublished = true;
  }

  public void SetParent(SpellCategoryEntity? parent)
  {
    Parent = parent;
    ParentId = parent?.SpellCategoryId;
    ParentUid = parent?.Id;
  }

  public void Unpublish(ContentLocaleUnpublished @event)
  {
    Update(@event);

    IsPublished = false;
  }

  public override string ToString() => $"{Name ?? Key} | {base.ToString()}";
}
