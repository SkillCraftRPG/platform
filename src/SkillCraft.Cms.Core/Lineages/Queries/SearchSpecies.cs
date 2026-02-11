using Krakenar.Contracts.Search;
using Logitar.CQRS;
using SkillCraft.Cms.Core.Lineages.Models;

namespace SkillCraft.Cms.Core.Lineages.Queries;

internal record SearchSpeciesQuery(SearchSpeciesPayload Payload) : IQuery<SearchResults<SpeciesModel>>;

internal class SearchSpeciesQueryHandler : IQueryHandler<SearchSpeciesQuery, SearchResults<SpeciesModel>>
{
  private readonly ISpeciesQuerier _speciesQuerier;

  public SearchSpeciesQueryHandler(ISpeciesQuerier speciesQuerier)
  {
    _speciesQuerier = speciesQuerier;
  }

  public async Task<SearchResults<SpeciesModel>> HandleAsync(SearchSpeciesQuery query, CancellationToken cancellationToken)
  {
    return await _speciesQuerier.SearchAsync(query.Payload, cancellationToken);
  }
}
