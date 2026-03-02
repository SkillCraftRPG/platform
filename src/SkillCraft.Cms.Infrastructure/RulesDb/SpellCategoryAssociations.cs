using Logitar.Data;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.RulesDb;

internal static class SpellCategoryAssociations
{
  public static readonly TableId Table = new(RulesContext.Schema, nameof(RulesContext.SpellCategoryAssociations), alias: null);

  public static readonly ColumnId SpellCategoryId = new(nameof(SpellCategoryAssociationEntity.SpellCategoryId), Table);
  public static readonly ColumnId SpellCategoryUid = new(nameof(SpellCategoryAssociationEntity.SpellCategoryUid), Table);
  public static readonly ColumnId SpellId = new(nameof(SpellCategoryAssociationEntity.SpellId), Table);
  public static readonly ColumnId SpellUid = new(nameof(SpellCategoryAssociationEntity.SpellUid), Table);
}
