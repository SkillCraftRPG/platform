using Krakenar.Contracts;
using Logitar.CQRS;
using SkillCraft.Cms.Core.Statistics.Models;

namespace SkillCraft.Cms.Core.Statistics.Queries;

internal record ReadStatisticQuery(Guid? Id, string? Slug) : IQuery<StatisticModel?>;

internal class ReadStatisticQueryHandler : IQueryHandler<ReadStatisticQuery, StatisticModel?>
{
  private readonly IStatisticQuerier _statisticQuerier;

  public ReadStatisticQueryHandler(IStatisticQuerier statisticQuerier)
  {
    _statisticQuerier = statisticQuerier;
  }

  public async Task<StatisticModel?> HandleAsync(ReadStatisticQuery query, CancellationToken cancellationToken)
  {
    Dictionary<Guid, StatisticModel> statistics = new(capacity: 2);

    if (query.Id.HasValue)
    {
      StatisticModel? statistic = await _statisticQuerier.ReadAsync(query.Id.Value, cancellationToken);
      if (statistic is not null)
      {
        statistics[statistic.Id] = statistic;
      }
    }

    if (!string.IsNullOrWhiteSpace(query.Slug))
    {
      StatisticModel? statistic = await _statisticQuerier.ReadAsync(query.Slug, cancellationToken);
      if (statistic is not null)
      {
        statistics[statistic.Id] = statistic;
      }
    }

    if (statistics.Count > 1)
    {
      throw TooManyResultsException<StatisticModel>.ExpectedSingle(statistics.Count);
    }

    return statistics.Values.SingleOrDefault();
  }
}
