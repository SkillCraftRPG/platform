using Krakenar.Contracts.Search;
using Logitar.CQRS;
using SkillCraft.Cms.Core.Attributes.Models;

namespace SkillCraft.Cms.Core.Attributes.Queries;

internal record SearchAttributesQuery(SearchAttributesPayload Payload) : IQuery<SearchResults<AttributeModel>>;

internal class SearchAttributesQueryHandler : IQueryHandler<SearchAttributesQuery, SearchResults<AttributeModel>>
{
  private readonly IAttributeQuerier _attributeQuerier;

  public SearchAttributesQueryHandler(IAttributeQuerier attributeQuerier)
  {
    _attributeQuerier = attributeQuerier;
  }

  public async Task<SearchResults<AttributeModel>> HandleAsync(SearchAttributesQuery query, CancellationToken cancellationToken)
  {
    return await _attributeQuerier.SearchAsync(query.Payload, cancellationToken);
  }
}
