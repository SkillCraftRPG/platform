using Krakenar.Contracts.Search;
using Logitar.CQRS;
using Microsoft.Extensions.DependencyInjection;
using SkillCraft.Cms.Core.Statistics.Models;
using SkillCraft.Cms.Core.Statistics.Queries;

namespace SkillCraft.Cms.Core.Statistics;

public interface IStatisticService
{
  Task<StatisticModel?> ReadAsync(Guid? id = null, string? slug = null, CancellationToken cancellationToken = default);
  Task<SearchResults<StatisticModel>> SearchAsync(SearchStatisticsPayload payload, CancellationToken cancellationToken = default);
}

internal class StatisticService : IStatisticService
{
  public static void Register(IServiceCollection services)
  {
    services.AddTransient<IStatisticService, StatisticService>();
    services.AddTransient<IQueryHandler<ReadStatisticQuery, StatisticModel?>, ReadStatisticQueryHandler>();
    services.AddTransient<IQueryHandler<SearchStatisticsQuery, SearchResults<StatisticModel>>, SearchStatisticsQueryHandler>();
  }

  private readonly IQueryBus _queryBus;

  public StatisticService(IQueryBus queryBus)
  {
    _queryBus = queryBus;
  }

  public async Task<StatisticModel?> ReadAsync(Guid? id, string? slug, CancellationToken cancellationToken)
  {
    ReadStatisticQuery query = new(id, slug);
    return await _queryBus.ExecuteAsync(query, cancellationToken);
  }

  public async Task<SearchResults<StatisticModel>> SearchAsync(SearchStatisticsPayload payload, CancellationToken cancellationToken)
  {
    SearchStatisticsQuery query = new(payload);
    return await _queryBus.ExecuteAsync(query, cancellationToken);
  }
}
