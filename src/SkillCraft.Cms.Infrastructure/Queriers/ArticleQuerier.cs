using Krakenar.Contracts.Actors;
using Krakenar.Core.Actors;
using Krakenar.EntityFrameworkCore.Relational.KrakenarDb;
using Logitar.EventSourcing;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Cms.Core.Articles;
using SkillCraft.Cms.Core.Articles.Models;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Queriers;

internal class ArticleQuerier : IArticleQuerier
{
  private readonly IActorService _actorService;
  private readonly DbSet<ArticleEntity> _articles;

  public ArticleQuerier(IActorService actorService, EncyclopediaContext encyclopedia)
  {
    _actorService = actorService;
    _articles = encyclopedia.Articles;
  }

  public async Task<ArticleModel?> ReadAsync(string collection, string path, CancellationToken cancellationToken)
  {
    string keyNormalized = Helper.Normalize(collection);
    string[] slugsNormalized = path.Split('/').Where(segment => !string.IsNullOrWhiteSpace(segment)).Select(Helper.Normalize).ToArray();
    if (string.IsNullOrEmpty(keyNormalized) || slugsNormalized.Length < 1)
    {
      return null;
    }

    var data = await _articles.AsNoTracking()
      .Where(x => x.Collection!.KeyNormalized == keyNormalized && slugsNormalized.Contains(x.SlugNormalized))
      .Select(x => new { x.SlugNormalized, x.ArticleId, x.ParentId })
      .ToArrayAsync(cancellationToken);

    Dictionary<string, ArticleRelation[]> grouped = data.GroupBy(x => x.SlugNormalized)
      .ToDictionary(x => x.Key, x => x.Select(ids => new ArticleRelation(ids.ArticleId, ids.ParentId)).ToArray());

    int? articleId = null;
    foreach (string slugNormalized in slugsNormalized)
    {
      if (!grouped.TryGetValue(slugNormalized, out ArticleRelation[]? articles))
      {
        return null;
      }

      int[] articleIds = articles.Where(x => x.ParentId == articleId).Select(x => x.Id).Distinct().ToArray();
      if (articleIds.Length != 1)
      {
        return null;
      }
      articleId = articleIds.Single();
    }

    if (!articleId.HasValue)
    {
      return null;
    }

    ArticleEntity? article = await _articles.AsNoTracking()
      .Where(x => x.ArticleId == articleId.Value && x.IsPublished)
      .SingleOrDefaultAsync(cancellationToken);
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

  private record ArticleRelation(int Id, int? ParentId);
}
