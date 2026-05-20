using Krakenar.Contracts.Actors;
using Krakenar.Contracts.Search;
using Krakenar.Core.Actors;
using Krakenar.EntityFrameworkCore.Relational;
using Krakenar.EntityFrameworkCore.Relational.KrakenarDb;
using Logitar.Data;
using Logitar.EventSourcing;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Cms.Core.Maps;
using SkillCraft.Cms.Core.Maps.Models;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Queriers;

internal class MapQuerier : IMapQuerier
{
  private readonly IActorService _actorService;
  private readonly DbSet<MapEntity> _maps;
  private readonly ISqlHelper _sqlHelper;

  public MapQuerier(IActorService actorService, EncyclopediaContext encyclopedia, ISqlHelper sqlHelper)
  {
    _actorService = actorService;
    _maps = encyclopedia.Maps;
    _sqlHelper = sqlHelper;
  }

  public async Task<MapModel?> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    MapEntity? map = await _maps.AsNoTracking()
      .Where(x => x.Id == id && x.IsPublished)
      .SingleOrDefaultAsync(cancellationToken);
    return map is null ? null : await MapAsync(map, cancellationToken);
  }

  public async Task<MapModel?> ReadAsync(string key, CancellationToken cancellationToken)
  {
    string keyNormalized = Helper.Normalize(key);
    MapEntity? map = await _maps.AsNoTracking()
      .Where(x => x.KeyNormalized == keyNormalized && x.IsPublished)
      .SingleOrDefaultAsync(cancellationToken);
    return map is null ? null : await MapAsync(map, cancellationToken);
  }

  public async Task<SearchResults<MapModel>> SearchAsync(SearchMapsPayload payload, CancellationToken cancellationToken)
  {
    IQueryBuilder builder = _sqlHelper.Query(EncyclopediaDb.Maps.Table).SelectAll(EncyclopediaDb.Maps.Table)
      .ApplyIdFilter(EncyclopediaDb.Maps.Id, payload.Ids)
      .Where(EncyclopediaDb.Maps.IsPublished, Operators.IsEqualTo(true));
    _sqlHelper.ApplyTextSearch(builder, payload.Search, EncyclopediaDb.Maps.Key, EncyclopediaDb.Maps.Title);

    if (payload.ArticleId.HasValue)
    {
      OperatorCondition condition = new(EncyclopediaDb.ArticleMaps.ArticleUid, Operators.IsEqualTo(payload.ArticleId.Value));
      builder.Join(EncyclopediaDb.ArticleMaps.MapId, EncyclopediaDb.Maps.MapId, condition);
    }

    IQueryable<MapEntity> query = _maps.FromQuery(builder).AsNoTracking();

    long total = await query.LongCountAsync(cancellationToken);

    IOrderedQueryable<MapEntity>? ordered = null;
    foreach (MapSortOption sort in payload.Sort)
    {
      switch (sort.Field)
      {
        case MapSort.CreatedOn:
          ordered = (ordered is null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.CreatedOn) : query.OrderBy(x => x.CreatedOn))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.CreatedOn) : ordered.ThenBy(x => x.CreatedOn));
          break;
        case MapSort.Key:
          ordered = (ordered is null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.Key) : query.OrderBy(x => x.Key))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.Key) : ordered.ThenBy(x => x.Key));
          break;
        case MapSort.Title:
          ordered = (ordered is null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.Title ?? x.Key) : query.OrderBy(x => x.Title ?? x.Key))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.Title ?? x.Key) : ordered.ThenBy(x => x.Title ?? x.Key));
          break;
        case MapSort.UpdatedOn:
          ordered = (ordered is null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.UpdatedOn) : query.OrderBy(x => x.UpdatedOn))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.UpdatedOn) : ordered.ThenBy(x => x.UpdatedOn));
          break;
      }
    }
    query = ordered ?? query;

    query = query.ApplyPaging(payload);

    MapEntity[] entities = await query.ToArrayAsync(cancellationToken);
    IReadOnlyCollection<MapModel> maps = await MapAsync(entities, cancellationToken);

    return new SearchResults<MapModel>(maps, total);
  }

  private async Task<MapModel> MapAsync(MapEntity map, CancellationToken cancellationToken)
  {
    return (await MapAsync([map], cancellationToken)).Single();
  }

  private async Task<IReadOnlyCollection<MapModel>> MapAsync(IEnumerable<MapEntity> maps, CancellationToken cancellationToken)
  {
    IEnumerable<ActorId> actorIds = maps.SelectMany(map => map.GetActorIds());
    IReadOnlyDictionary<ActorId, Actor> actors = await _actorService.FindAsync(actorIds, cancellationToken);
    EncyclopediaMapper mapper = new(actors);

    return maps.Select(mapper.ToMap).ToList().AsReadOnly();
  }
}
