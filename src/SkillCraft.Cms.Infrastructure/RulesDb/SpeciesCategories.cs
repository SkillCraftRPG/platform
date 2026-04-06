using Logitar.Data;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.RulesDb;

internal static class SpeciesCategories
{
  public static readonly TableId Table = new(RulesContext.Schema, nameof(RulesContext.SpeciesCategories), alias: null);

  public static readonly ColumnId CreatedBy = new(nameof(SpeciesCategoryEntity.CreatedBy), Table);
  public static readonly ColumnId CreatedOn = new(nameof(SpeciesCategoryEntity.CreatedOn), Table);
  public static readonly ColumnId StreamId = new(nameof(SpeciesCategoryEntity.StreamId), Table);
  public static readonly ColumnId UpdatedBy = new(nameof(SpeciesCategoryEntity.UpdatedBy), Table);
  public static readonly ColumnId UpdatedOn = new(nameof(SpeciesCategoryEntity.UpdatedOn), Table);
  public static readonly ColumnId Version = new(nameof(SpeciesCategoryEntity.Version), Table);

  public static readonly ColumnId Columns = new(nameof(SpeciesCategoryEntity.Columns), Table);
  public static readonly ColumnId HtmlContent = new(nameof(SpeciesCategoryEntity.HtmlContent), Table);
  public static readonly ColumnId Id = new(nameof(SpeciesCategoryEntity.Id), Table);
  public static readonly ColumnId IsPublished = new(nameof(SpeciesCategoryEntity.IsPublished), Table);
  public static readonly ColumnId Key = new(nameof(SpeciesCategoryEntity.Key), Table);
  public static readonly ColumnId KeyNormalized = new(nameof(SpeciesCategoryEntity.KeyNormalized), Table);
  public static readonly ColumnId Name = new(nameof(SpeciesCategoryEntity.Name), Table);
  public static readonly ColumnId Order = new(nameof(SpeciesCategoryEntity.Order), Table);
  public static readonly ColumnId SpeciesCategoryId = new(nameof(SpeciesCategoryEntity.SpeciesCategoryId), Table);
}
