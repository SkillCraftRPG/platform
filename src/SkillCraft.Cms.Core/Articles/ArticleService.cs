using Logitar.CQRS;
using Microsoft.Extensions.DependencyInjection;
using SkillCraft.Cms.Core.Articles.Models;
using SkillCraft.Cms.Core.Articles.Queries;

namespace SkillCraft.Cms.Core.Articles;

public interface IArticleService
{
  Task<ArticleModel?> ReadAsync(string collection, string path, CancellationToken cancellationToken = default);
}

internal class ArticleService : IArticleService
{
  public static void Register(IServiceCollection services)
  {
    services.AddTransient<IArticleService, ArticleService>();
    services.AddTransient<IQueryHandler<ReadArticleQuery, ArticleModel?>, ReadArticleQueryHandler>();
  }

  private readonly IQueryBus _queryBus;

  public ArticleService(IQueryBus queryBus)
  {
    _queryBus = queryBus;
  }

  public async Task<ArticleModel?> ReadAsync(string collection, string path, CancellationToken cancellationToken)
  {
    ReadArticleQuery query = new(collection, path);
    return await _queryBus.ExecuteAsync(query, cancellationToken);
  }
}
