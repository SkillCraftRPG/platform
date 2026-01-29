using Krakenar.Contracts;
using Logitar.CQRS;
using SkillCraft.Cms.Core.Collections.Models;

namespace SkillCraft.Cms.Core.Collections.Queries;

internal record ReadCollectionQuery(Guid? Id, string? Slug) : IQuery<CollectionModel?>;

internal class ReadCollectionQueryHandler : IQueryHandler<ReadCollectionQuery, CollectionModel?>
{
  private readonly ICollectionQuerier _collectionQuerier;

  public ReadCollectionQueryHandler(ICollectionQuerier collectionQuerier)
  {
    _collectionQuerier = collectionQuerier;
  }

  public async Task<CollectionModel?> HandleAsync(ReadCollectionQuery query, CancellationToken cancellationToken)
  {
    Dictionary<Guid, CollectionModel> collections = new(capacity: 2);

    if (query.Id.HasValue)
    {
      CollectionModel? collection = await _collectionQuerier.ReadAsync(query.Id.Value, cancellationToken);
      if (collection is not null)
      {
        collections[collection.Id] = collection;
      }
    }

    if (!string.IsNullOrWhiteSpace(query.Slug))
    {
      CollectionModel? collection = await _collectionQuerier.ReadAsync(query.Slug, cancellationToken);
      if (collection is not null)
      {
        collections[collection.Id] = collection;
      }
    }

    if (collections.Count > 1)
    {
      throw TooManyResultsException<CollectionModel>.ExpectedSingle(collections.Count);
    }

    return collections.Values.SingleOrDefault();
  }
}
