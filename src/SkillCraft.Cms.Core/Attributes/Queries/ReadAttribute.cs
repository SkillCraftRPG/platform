using Krakenar.Contracts;
using Logitar.CQRS;
using SkillCraft.Cms.Core.Attributes.Models;

namespace SkillCraft.Cms.Core.Attributes.Queries;

internal record ReadAttributeQuery(Guid? Id, string? Slug) : IQuery<AttributeModel?>;

internal class ReadAttributeQueryHandler : IQueryHandler<ReadAttributeQuery, AttributeModel?>
{
  private readonly IAttributeQuerier _attributeQuerier;

  public ReadAttributeQueryHandler(IAttributeQuerier attributeQuerier)
  {
    _attributeQuerier = attributeQuerier;
  }

  public async Task<AttributeModel?> HandleAsync(ReadAttributeQuery query, CancellationToken cancellationToken)
  {
    Dictionary<Guid, AttributeModel> attributes = new(capacity: 2);

    if (query.Id.HasValue)
    {
      AttributeModel? attribute = await _attributeQuerier.ReadAsync(query.Id.Value, cancellationToken);
      if (attribute is not null)
      {
        attributes[attribute.Id] = attribute;
      }
    }

    if (!string.IsNullOrWhiteSpace(query.Slug))
    {
      AttributeModel? attribute = await _attributeQuerier.ReadAsync(query.Slug, cancellationToken);
      if (attribute is not null)
      {
        attributes[attribute.Id] = attribute;
      }
    }

    if (attributes.Count > 1)
    {
      throw TooManyResultsException<AttributeModel>.ExpectedSingle(attributes.Count);
    }

    return attributes.Values.SingleOrDefault();
  }
}
