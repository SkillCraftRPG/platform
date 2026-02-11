using Krakenar.Contracts.Search;
using Logitar.CQRS;
using SkillCraft.Cms.Core.Lineages.Models;

namespace SkillCraft.Cms.Core.Lineages.Queries;

internal record SearchEthnicitiesQuery(SearchEthnicitiesPayload Payload) : IQuery<SearchResults<EthnicityModel>>;

internal class SearchEthnicitiesQueryHandler : IQueryHandler<SearchEthnicitiesQuery, SearchResults<EthnicityModel>>
{
  private readonly IEthnicityQuerier _ethnicityQuerier;

  public SearchEthnicitiesQueryHandler(IEthnicityQuerier ethnicityQuerier)
  {
    _ethnicityQuerier = ethnicityQuerier;
  }

  public async Task<SearchResults<EthnicityModel>> HandleAsync(SearchEthnicitiesQuery query, CancellationToken cancellationToken)
  {
    return await _ethnicityQuerier.SearchAsync(query.Payload, cancellationToken);
  }
}
