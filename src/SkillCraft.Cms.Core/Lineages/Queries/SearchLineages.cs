using Krakenar.Contracts.Search;
using Logitar.CQRS;
using SkillCraft.Cms.Core.Lineages.Models;

namespace SkillCraft.Cms.Core.Lineages.Queries;

internal record SearchLineagesQuery(SearchLineagesPayload Payload) : IQuery<SearchResults<LineageModel>>;

internal class SearchLineagesQueryHandler : IQueryHandler<SearchLineagesQuery, SearchResults<LineageModel>>
{
  private readonly ILineageQuerier _lineageQuerier;

  public SearchLineagesQueryHandler(ILineageQuerier lineageQuerier)
  {
    _lineageQuerier = lineageQuerier;
  }

  public async Task<SearchResults<LineageModel>> HandleAsync(SearchLineagesQuery query, CancellationToken cancellationToken)
  {
    return await _lineageQuerier.SearchAsync(query.Payload, cancellationToken);
  }
}
