namespace SkillCraft.Cms.Infrastructure.Entities;

internal class ArticleMapEntity
{
  public ArticleEntity? Article { get; private set; }
  public int ArticleId { get; private set; }
  public Guid ArticleUid { get; private set; }

  public MapEntity? Map { get; private set; }
  public int MapId { get; private set; }
  public Guid MapUid { get; private set; }

  public ArticleMapEntity(ArticleEntity article, MapEntity map)
  {
    Article = article;
    ArticleId = article.ArticleId;
    ArticleUid = article.Id;

    Map = map;
    MapId = map.MapId;
    MapUid = map.Id;
  }

  private ArticleMapEntity()
  {
  }

  public override bool Equals(object? obj) => obj is ArticleMapEntity entity && entity.ArticleId == ArticleId && entity.MapId == MapId;
  public override int GetHashCode() => HashCode.Combine(ArticleId, MapId);
  public override string ToString() => $"{GetType()} (ArticleId={ArticleId}, MapId={MapId})";
}
