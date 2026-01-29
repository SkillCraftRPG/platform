using Krakenar.Contracts.Actors;
using Krakenar.Core.Actors;
using Krakenar.EntityFrameworkCore.Relational.KrakenarDb;
using Logitar.EventSourcing;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Cms.Core.Articles;
using SkillCraft.Cms.Core.Articles.Models;
using SkillCraft.Cms.Infrastructure.Configurations;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Queriers;

internal class ArticleQuerier : IArticleQuerier
{
  private readonly IActorService _actorService;
  private readonly DbSet<ArticleEntity> _articles;
  private readonly DbSet<ArticleHierarchyEntity> _articleHierarchy;

  public ArticleQuerier(IActorService actorService, EncyclopediaContext encyclopedia)
  {
    _actorService = actorService;
    _articles = encyclopedia.Articles;
    _articleHierarchy = encyclopedia.ArticleHierarchy;
  }

  public async Task<ArticleModel?> ReadAsync(string collection, string path, CancellationToken cancellationToken)
  {
    string collectionNormalized = Helper.Normalize(collection);
    string pathNormalized = Helper.Normalize(path);

    string? idPath = await _articleHierarchy.AsNoTracking()
      .Where(x => x.Collection!.KeyNormalized == collectionNormalized && x.SlugPath == pathNormalized)
      .Select(x => x.IdPath)
      .SingleOrDefaultAsync(cancellationToken);
    if (idPath is null)
    {
      return null;
    }

    int[] ids = idPath.Split(Constants.PathSeparator).Select(int.Parse).ToArray();
    Dictionary<int, ArticleEntity> articles = await _articles.AsNoTracking()
      .Where(x => ids.Contains(x.ArticleId) && x.IsPublished && x.Collection!.IsPublished)
      .ToDictionaryAsync(x => x.ArticleId, x => x, cancellationToken);

    ArticleEntity? parent = null;
    ArticleEntity? article = null;
    foreach (int id in ids)
    {
      if (!articles.TryGetValue(id, out article))
      {
        return null;
      }

      article.Parent = parent;
      parent = article;
    }

    return article is null ? null : await MapAsync(article, cancellationToken);
  }

  private async Task<ArticleModel> MapAsync(ArticleEntity article, CancellationToken cancellationToken)
  {
    return (await MapAsync([article], cancellationToken)).Single();
  }
  private async Task<IReadOnlyCollection<ArticleModel>> MapAsync(IEnumerable<ArticleEntity> articles, CancellationToken cancellationToken)
  {
    IEnumerable<ActorId> actorIds = articles.SelectMany(article => article.GetActorIds());
    IReadOnlyDictionary<ActorId, Actor> actors = await _actorService.FindAsync(actorIds, cancellationToken);
    EncyclopediaMapper mapper = new(actors);

    return articles.Select(mapper.ToArticle).ToList().AsReadOnly();
  }
}
