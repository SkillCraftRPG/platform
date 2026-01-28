using Krakenar.Contracts.Search;
using Logitar.CQRS;
using SkillCraft.Cms.Core.Statistics.Models;

namespace SkillCraft.Cms.Core.Statistics.Queries;

internal record SearchStatisticsQuery(SearchStatisticsPayload Payload) : IQuery<SearchResults<StatisticModel>>;

internal class SearchStatisticsQueryHandler : IQueryHandler<SearchStatisticsQuery, SearchResults<StatisticModel>>
{
  private readonly IStatisticQuerier _statisticQuerier;

  public SearchStatisticsQueryHandler(IStatisticQuerier statisticQuerier)
  {
    _statisticQuerier = statisticQuerier;
  }

  public async Task<SearchResults<StatisticModel>> HandleAsync(SearchStatisticsQuery query, CancellationToken cancellationToken)
  {
    return await _statisticQuerier.SearchAsync(query.Payload, cancellationToken);
  }
}
