using Krakenar.Core.Contents;
using Krakenar.Core.Contents.Events;
using Krakenar.EntityFrameworkCore.Relational.KrakenarDb;
using SkillCraft.Cms.Core.Spells;
using AggregateEntity = Krakenar.EntityFrameworkCore.Relational.Entities.Aggregate;

namespace SkillCraft.Cms.Infrastructure.Entities;

internal class SpellEffectEntity : AggregateEntity
{
  public int SpellEffectId { get; private set; }
  public Guid Id { get; private set; }

  public bool IsPublished { get; private set; }

  public string Key { get; set; } = string.Empty;
  public string KeyNormalized
  {
    get => Helper.Normalize(Key);
    private set { }
  }
  public string? Name { get; set; }

  public SpellEntity? Spell { get; private set; }
  public int SpellId { get; private set; }
  public Guid SpellUid { get; private set; }

  public int Level { get; set; }

  public string CastingTime { get; set; } = string.Empty;
  public bool IsRitual { get; set; }

  public int? Duration { get; set; }
  public TimeUnit? DurationUnit { get; set; }
  public bool IsConcentration { get; set; }

  public int Range { get; set; }

  public bool IsSomatic { get; set; }
  public bool IsVerbal { get; set; }
  public string? Focus { get; set; }
  public string? Material { get; set; }

  public string? HtmlContent { get; set; }

  public SpellEffectEntity(ContentLocalePublished @event) : base(@event)
  {
    Id = new ContentId(@event.StreamId).EntityId;
  }

  private SpellEffectEntity() : base()
  {
  }

  public void Publish(ContentLocalePublished @event)
  {
    Update(@event);

    IsPublished = true;
  }

  public void SetSpell(SpellEntity spell)
  {
    Spell = spell;
    SpellId = spell.SpellId;
    SpellUid = spell.Id;
  }

  public void Unpublish(ContentLocaleUnpublished @event)
  {
    Update(@event);

    IsPublished = false;
  }

  public override string ToString() => $"{Name ?? Key} | {base.ToString()}";
}
