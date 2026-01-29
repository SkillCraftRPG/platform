using SkillCraft.Cms.Core.Collections.Models;

namespace SkillCraft.Cms.Core.Collections;

public interface ICollectionQuerier
{
  Task<CollectionModel?> ReadAsync(Guid id, CancellationToken cancellationToken = default);
  Task<CollectionModel?> ReadAsync(string slug, CancellationToken cancellationToken = default);
}
