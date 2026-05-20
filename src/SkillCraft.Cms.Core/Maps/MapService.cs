using Krakenar.Contracts.Search;
using Logitar.CQRS;
using Microsoft.Extensions.DependencyInjection;
using SkillCraft.Cms.Core.Maps.Models;
using SkillCraft.Cms.Core.Maps.Queries;

namespace SkillCraft.Cms.Core.Maps;

public interface IMapService
{
  Task<MapModel?> ReadAsync(Guid? id = null, string? key = null, CancellationToken cancellationToken = default);
  Task<SearchResults<MapModel>> SearchAsync(SearchMapsPayload payload, CancellationToken cancellationToken = default);
}

internal class MapService : IMapService
{
  public static void Register(IServiceCollection services)
  {
    services.AddTransient<IMapService, MapService>();
    services.AddTransient<IQueryHandler<ReadMapQuery, MapModel?>, ReadMapQueryHandler>();
    services.AddTransient<IQueryHandler<SearchMapsQuery, SearchResults<MapModel>>, SearchMapsQueryHandler>();
  }

  private readonly IQueryBus _queryBus;

  public MapService(IQueryBus queryBus)
  {
    _queryBus = queryBus;
  }

  public async Task<MapModel?> ReadAsync(Guid? id, string? key, CancellationToken cancellationToken)
  {
    ReadMapQuery query = new(id, key);
    return await _queryBus.ExecuteAsync(query, cancellationToken);
  }

  public async Task<SearchResults<MapModel>> SearchAsync(SearchMapsPayload payload, CancellationToken cancellationToken)
  {
    SearchMapsQuery query = new(payload);
    return await _queryBus.ExecuteAsync(query, cancellationToken);
  }
}
