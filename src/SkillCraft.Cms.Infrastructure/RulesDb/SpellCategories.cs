using Logitar.Data;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.RulesDb;

internal static class SpellCategories
{
  public static readonly TableId Table = new(RulesContext.Schema, nameof(RulesContext.SpellCategories), alias: null);

  public static readonly ColumnId CreatedBy = new(nameof(SpellCategoryEntity.CreatedBy), Table);
  public static readonly ColumnId CreatedOn = new(nameof(SpellCategoryEntity.CreatedOn), Table);
  public static readonly ColumnId StreamId = new(nameof(SpellCategoryEntity.StreamId), Table);
  public static readonly ColumnId UpdatedBy = new(nameof(SpellCategoryEntity.UpdatedBy), Table);
  public static readonly ColumnId UpdatedOn = new(nameof(SpellCategoryEntity.UpdatedOn), Table);
  public static readonly ColumnId Version = new(nameof(SpellCategoryEntity.Version), Table);

  public static readonly ColumnId Id = new(nameof(SpellCategoryEntity.Id), Table);
  public static readonly ColumnId IsPublished = new(nameof(SpellCategoryEntity.IsPublished), Table);
  public static readonly ColumnId Key = new(nameof(SpellCategoryEntity.Key), Table);
  public static readonly ColumnId KeyNormalized = new(nameof(SpellCategoryEntity.KeyNormalized), Table);
  public static readonly ColumnId Name = new(nameof(SpellCategoryEntity.Name), Table);
  public static readonly ColumnId ParentId = new(nameof(SpellCategoryEntity.ParentId), Table);
  public static readonly ColumnId ParentUid = new(nameof(SpellCategoryEntity.ParentUid), Table);
  public static readonly ColumnId SpellCategoryId = new(nameof(SpellCategoryEntity.SpellCategoryId), Table);
}
