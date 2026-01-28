using Krakenar.Contracts.Search;
using Logitar.CQRS;
using SkillCraft.Cms.Core.Educations.Models;

namespace SkillCraft.Cms.Core.Educations.Queries;

internal record SearchEducationsQuery(SearchEducationsPayload Payload) : IQuery<SearchResults<EducationModel>>;

internal class SearchEducationsQueryHandler : IQueryHandler<SearchEducationsQuery, SearchResults<EducationModel>>
{
  private readonly IEducationQuerier _educationQuerier;

  public SearchEducationsQueryHandler(IEducationQuerier educationQuerier)
  {
    _educationQuerier = educationQuerier;
  }

  public async Task<SearchResults<EducationModel>> HandleAsync(SearchEducationsQuery query, CancellationToken cancellationToken)
  {
    return await _educationQuerier.SearchAsync(query.Payload, cancellationToken);
  }
}
