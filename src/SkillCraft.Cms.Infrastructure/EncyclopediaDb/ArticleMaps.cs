using Logitar.Data;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.EncyclopediaDb;

internal static class ArticleMaps
{
  public static readonly TableId Table = new(EncyclopediaContext.Schema, nameof(EncyclopediaContext.ArticleMaps), alias: null);

  public static readonly ColumnId ArticleId = new(nameof(ArticleMapEntity.ArticleId), Table);
  public static readonly ColumnId ArticleUid = new(nameof(ArticleMapEntity.ArticleUid), Table);
  public static readonly ColumnId MapId = new(nameof(ArticleMapEntity.MapId), Table);
  public static readonly ColumnId MapUid = new(nameof(ArticleMapEntity.MapUid), Table);
}
