using Krakenar.Contracts;
using Logitar.CQRS;
using SkillCraft.Cms.Core.Maps.Models;

namespace SkillCraft.Cms.Core.Maps.Queries;

internal record ReadMapQuery(Guid? Id, string? Key) : IQuery<MapModel?>;

internal class ReadMapQueryHandler : IQueryHandler<ReadMapQuery, MapModel?>
{
  private readonly IMapQuerier _mapQuerier;

  public ReadMapQueryHandler(IMapQuerier mapQuerier)
  {
    _mapQuerier = mapQuerier;
  }

  public async Task<MapModel?> HandleAsync(ReadMapQuery query, CancellationToken cancellationToken)
  {
    Dictionary<Guid, MapModel> maps = new(capacity: 2);

    if (query.Id.HasValue)
    {
      MapModel? map = await _mapQuerier.ReadAsync(query.Id.Value, cancellationToken);
      if (map is not null)
      {
        maps[map.Id] = map;
      }
    }

    if (!string.IsNullOrWhiteSpace(query.Key))
    {
      MapModel? map = await _mapQuerier.ReadAsync(query.Key, cancellationToken);
      if (map is not null)
      {
        maps[map.Id] = map;
      }
    }

    if (maps.Count > 1)
    {
      throw TooManyResultsException<MapModel>.ExpectedSingle(maps.Count);
    }

    return maps.Values.SingleOrDefault();
  }
}
