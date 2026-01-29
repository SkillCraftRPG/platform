using Krakenar.Contracts.Actors;
using Krakenar.Core.Actors;
using Krakenar.EntityFrameworkCore.Relational.KrakenarDb;
using Logitar.Data;
using Logitar.EventSourcing;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Cms.Core.Collections;
using SkillCraft.Cms.Core.Collections.Models;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Queriers;

internal class CollectionQuerier : ICollectionQuerier
{
  private readonly IActorService _actorService;
  private readonly DbSet<CollectionEntity> _collections;

  public CollectionQuerier(IActorService actorService, EncyclopediaContext encyclopedia)
  {
    _actorService = actorService;
    _collections = encyclopedia.Collections;
  }

  public async Task<CollectionModel?> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    CollectionEntity? collection = await _collections.AsNoTracking()
      .Where(x => x.Id == id && x.IsPublished)
      .SingleOrDefaultAsync(cancellationToken);
    return collection is null ? null : await MapAsync(collection, cancellationToken);
  }
  public async Task<CollectionModel?> ReadAsync(string slug, CancellationToken cancellationToken)
  {
    string slugNormalized = Helper.Normalize(slug);
    CollectionEntity? collection = await _collections.AsNoTracking()
      .Where(x => x.SlugNormalized == slugNormalized && x.IsPublished)
      .SingleOrDefaultAsync(cancellationToken);
    return collection is null ? null : await MapAsync(collection, cancellationToken);
  }

  private async Task<CollectionModel> MapAsync(CollectionEntity collection, CancellationToken cancellationToken)
  {
    return (await MapAsync([collection], cancellationToken)).Single();
  }
  private async Task<IReadOnlyCollection<CollectionModel>> MapAsync(IEnumerable<CollectionEntity> collections, CancellationToken cancellationToken)
  {
    IEnumerable<ActorId> actorIds = collections.SelectMany(collection => collection.GetActorIds());
    IReadOnlyDictionary<ActorId, Actor> actors = await _actorService.FindAsync(actorIds, cancellationToken);
    EncyclopediaMapper mapper = new(actors);

    return collections.Select(mapper.ToCollection).ToList().AsReadOnly();
  }
}
