using Krakenar.Contracts.Search;
using Logitar.CQRS;
using SkillCraft.Cms.Core.Skills.Models;

namespace SkillCraft.Cms.Core.Skills.Queries;

internal record SearchSkillsQuery(SearchSkillsPayload Payload) : IQuery<SearchResults<SkillModel>>;

internal class SearchSkillsQueryHandler : IQueryHandler<SearchSkillsQuery, SearchResults<SkillModel>>
{
  private readonly ISkillQuerier _skillQuerier;

  public SearchSkillsQueryHandler(ISkillQuerier skillQuerier)
  {
    _skillQuerier = skillQuerier;
  }

  public async Task<SearchResults<SkillModel>> HandleAsync(SearchSkillsQuery query, CancellationToken cancellationToken)
  {
    return await _skillQuerier.SearchAsync(query.Payload, cancellationToken);
  }
}
