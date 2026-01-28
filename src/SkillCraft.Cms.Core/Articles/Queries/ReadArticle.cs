using Logitar.CQRS;
using SkillCraft.Cms.Core.Articles.Models;

namespace SkillCraft.Cms.Core.Articles.Queries;

internal record ReadArticleQuery(string Collection, string Path) : IQuery<ArticleModel?>;

internal class ReadArticleQueryHandler : IQueryHandler<ReadArticleQuery, ArticleModel?>
{
  private readonly IArticleQuerier _articleQuerier;

  public ReadArticleQueryHandler(IArticleQuerier articleQuerier)
  {
    _articleQuerier = articleQuerier;
  }

  public async Task<ArticleModel?> HandleAsync(ReadArticleQuery query, CancellationToken cancellationToken)
  {
    return await _articleQuerier.ReadAsync(query.Collection, query.Path, cancellationToken);
  }
}
