using Logitar.CQRS;
using Microsoft.Extensions.DependencyInjection;
using SkillCraft.Cms.Core.Collections.Models;
using SkillCraft.Cms.Core.Collections.Queries;

namespace SkillCraft.Cms.Core.Collections;

public interface ICollectionService
{
  Task<CollectionModel?> ReadAsync(Guid? id = null, string? slug = null, CancellationToken cancellationToken = default);
}

internal class CollectionService : ICollectionService
{
  public static void Register(IServiceCollection services)
  {
    services.AddTransient<ICollectionService, CollectionService>();
    services.AddTransient<IQueryHandler<ReadCollectionQuery, CollectionModel?>, ReadCollectionQueryHandler>();
  }

  private readonly IQueryBus _queryBus;

  public CollectionService(IQueryBus queryBus)
  {
    _queryBus = queryBus;
  }

  public async Task<CollectionModel?> ReadAsync(Guid? id, string? slug, CancellationToken cancellationToken)
  {
    ReadCollectionQuery query = new(id, slug);
    return await _queryBus.ExecuteAsync(query, cancellationToken);
  }
}
