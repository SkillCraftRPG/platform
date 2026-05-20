using Krakenar.Contracts.Search;
using SkillCraft.Cms.Core.Maps.Models;

namespace SkillCraft.Cms.Core.Maps;

public interface IMapQuerier
{
  Task<MapModel?> ReadAsync(Guid id, CancellationToken cancellationToken = default);
  Task<MapModel?> ReadAsync(string key, CancellationToken cancellationToken = default);

  Task<SearchResults<MapModel>> SearchAsync(SearchMapsPayload payload, CancellationToken cancellationToken = default);
}
