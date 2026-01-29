using Logitar.Data;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.EncyclopediaDb;

internal static class ArticleHierarchy
{
  public static readonly TableId Table = new(EncyclopediaContext.Schema, nameof(EncyclopediaContext.ArticleHierarchy), alias: null);

  public static readonly ColumnId ArticleId = new(nameof(ArticleHierarchyEntity.ArticleId), Table);
  public static readonly ColumnId ArticleUid = new(nameof(ArticleHierarchyEntity.ArticleUid), Table);
  public static readonly ColumnId CollectionId = new(nameof(ArticleHierarchyEntity.CollectionId), Table);
  public static readonly ColumnId CollectionUid = new(nameof(ArticleHierarchyEntity.CollectionUid), Table);
  public static readonly ColumnId Depth = new(nameof(ArticleHierarchyEntity.Depth), Table);
  public static readonly ColumnId IdPath = new(nameof(ArticleHierarchyEntity.IdPath), Table);
  public static readonly ColumnId SlugPath = new(nameof(ArticleHierarchyEntity.SlugPath), Table);
}
