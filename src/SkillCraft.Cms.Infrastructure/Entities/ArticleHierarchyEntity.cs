using SkillCraft.Cms.Infrastructure.Configurations;

namespace SkillCraft.Cms.Infrastructure.Entities;

internal class ArticleHierarchyEntity
{
  public ArticleEntity? Article { get; private set; }
  public int ArticleId { get; private set; }
  public Guid ArticleUid { get; private set; }

  public CollectionEntity? Collection { get; private set; }
  public int CollectionId { get; private set; }
  public Guid CollectionUid { get; private set; }

  public int Depth { get; private set; }
  public string IdPath { get; private set; } = string.Empty;
  public string SlugPath { get; private set; } = string.Empty;

  public ArticleHierarchyEntity(ArticleEntity article, IEnumerable<KeyValuePair<int, string>> path)
  {
    Article = article;
    ArticleId = article.ArticleId;
    ArticleUid = article.Id;

    Collection = article.Collection;
    CollectionId = article.CollectionId;
    CollectionUid = article.CollectionUid;

    Update(path);
  }

  private ArticleHierarchyEntity()
  {
  }

  public IReadOnlyList<KeyValuePair<int, string>> GetPath()
  {
    List<KeyValuePair<int, string>> path = new(capacity: Depth);
    string[] ids = IdPath.Split(Constants.PathSeparator);
    string[] slugs = SlugPath.Split(Constants.PathSeparator);
    for (int i = 0; i < Depth; i++)
    {
      path.Add(new KeyValuePair<int, string>(int.Parse(ids[i]), slugs[i]));
    }
    return path.AsReadOnly();
  }

  public void Update(IEnumerable<KeyValuePair<int, string>> path)
  {
    if (!path.Any())
    {
      throw new ArgumentException("The path should contain at least one node.", nameof(path));
    }

    Depth = path.Count();
    IdPath = string.Join(Constants.PathSeparator, path.Select(x => x.Key));
    SlugPath = string.Join(Constants.PathSeparator, path.Select(x => x.Value));
  }

  public override bool Equals(object? obj) => obj is ArticleHierarchyEntity hierarchy && hierarchy.ArticleId == ArticleId;
  public override int GetHashCode() => HashCode.Combine(ArticleId);
  public override string ToString() => $"{GetType()} (ArticleId={ArticleId})";
}
