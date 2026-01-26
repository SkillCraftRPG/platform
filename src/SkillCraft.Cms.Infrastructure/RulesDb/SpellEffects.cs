using Logitar.Data;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.RulesDb;

internal static class SpellEffects
{
  public static readonly TableId Table = new(RulesContext.Schema, nameof(RulesContext.SpellEffects), alias: null);

  public static readonly ColumnId CreatedBy = new(nameof(SpellEffectEntity.CreatedBy), Table);
  public static readonly ColumnId CreatedOn = new(nameof(SpellEffectEntity.CreatedOn), Table);
  public static readonly ColumnId StreamId = new(nameof(SpellEffectEntity.StreamId), Table);
  public static readonly ColumnId UpdatedBy = new(nameof(SpellEffectEntity.UpdatedBy), Table);
  public static readonly ColumnId UpdatedOn = new(nameof(SpellEffectEntity.UpdatedOn), Table);
  public static readonly ColumnId Version = new(nameof(SpellEffectEntity.Version), Table);

  public static readonly ColumnId CastingTime = new(nameof(SpellEffectEntity.CastingTime), Table);
  public static readonly ColumnId Duration = new(nameof(SpellEffectEntity.Duration), Table);
  public static readonly ColumnId DurationUnit = new(nameof(SpellEffectEntity.DurationUnit), Table);
  public static readonly ColumnId Focus = new(nameof(SpellEffectEntity.Focus), Table);
  public static readonly ColumnId HtmlContent = new(nameof(SpellEffectEntity.HtmlContent), Table);
  public static readonly ColumnId Id = new(nameof(SpellEffectEntity.Id), Table);
  public static readonly ColumnId IsConcentration = new(nameof(SpellEffectEntity.IsConcentration), Table);
  public static readonly ColumnId IsPublished = new(nameof(SpellEffectEntity.IsPublished), Table);
  public static readonly ColumnId IsRitual = new(nameof(SpellEffectEntity.IsRitual), Table);
  public static readonly ColumnId IsSomatic = new(nameof(SpellEffectEntity.IsSomatic), Table);
  public static readonly ColumnId IsVerbal = new(nameof(SpellEffectEntity.IsVerbal), Table);
  public static readonly ColumnId Key = new(nameof(SpellEffectEntity.Key), Table);
  public static readonly ColumnId KeyNormalized = new(nameof(SpellEffectEntity.KeyNormalized), Table);
  public static readonly ColumnId Level = new(nameof(SpellEffectEntity.Level), Table);
  public static readonly ColumnId Material = new(nameof(SpellEffectEntity.Material), Table);
  public static readonly ColumnId Name = new(nameof(SpellEffectEntity.Name), Table);
  public static readonly ColumnId Range = new(nameof(SpellEffectEntity.Range), Table);
  public static readonly ColumnId SpellEffectId = new(nameof(SpellEffectEntity.SpellEffectId), Table);
  public static readonly ColumnId SpellId = new(nameof(SpellEffectEntity.SpellId), Table);
  public static readonly ColumnId SpellUid = new(nameof(SpellEffectEntity.SpellUid), Table);
}
