using Krakenar.Contracts.Search;
using Logitar.CQRS;
using Microsoft.Extensions.DependencyInjection;
using SkillCraft.Cms.Core.Lineages.Models;
using SkillCraft.Cms.Core.Lineages.Queries;

namespace SkillCraft.Cms.Core.Lineages;

public interface ILineageService
{
  Task<LineageModel?> ReadAsync(Guid id, CancellationToken cancellationToken = default);
  Task<SearchResults<LineageModel>> SearchAsync(SearchLineagesPayload payload, CancellationToken cancellationToken = default);
}

internal class LineageService : ILineageService
{
  public static void Register(IServiceCollection services)
  {
    services.AddTransient<ILineageService, LineageService>();
    services.AddTransient<IQueryHandler<ReadLineageQuery, LineageModel?>, ReadLineageQueryHandler>();
    services.AddTransient<IQueryHandler<SearchLineagesQuery, SearchResults<LineageModel>>, SearchLineagesQueryHandler>();
  }

  private readonly IQueryBus _queryBus;

  public LineageService(IQueryBus queryBus)
  {
    _queryBus = queryBus;
  }

  public async Task<LineageModel?> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    ReadLineageQuery query = new(id);
    return await _queryBus.ExecuteAsync(query, cancellationToken);
  }

  public async Task<SearchResults<LineageModel>> SearchAsync(SearchLineagesPayload payload, CancellationToken cancellationToken)
  {
    SearchLineagesQuery query = new(payload);
    return await _queryBus.ExecuteAsync(query, cancellationToken);
  }
}
