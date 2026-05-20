using Krakenar.Contracts.Search;
using Logitar.CQRS;
using SkillCraft.Cms.Core.Maps.Models;

namespace SkillCraft.Cms.Core.Maps.Queries;

internal record SearchMapsQuery(SearchMapsPayload Payload) : IQuery<SearchResults<MapModel>>;

internal class SearchMapsQueryHandler : IQueryHandler<SearchMapsQuery, SearchResults<MapModel>>
{
  private readonly IMapQuerier _mapQuerier;

  public SearchMapsQueryHandler(IMapQuerier mapQuerier)
  {
    _mapQuerier = mapQuerier;
  }

  public async Task<SearchResults<MapModel>> HandleAsync(SearchMapsQuery query, CancellationToken cancellationToken)
  {
    return await _mapQuerier.SearchAsync(query.Payload, cancellationToken);
  }
}
