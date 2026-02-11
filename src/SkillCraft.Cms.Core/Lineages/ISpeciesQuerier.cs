using Krakenar.Contracts.Search;
using SkillCraft.Cms.Core.Lineages.Models;

namespace SkillCraft.Cms.Core.Lineages;

public interface ISpeciesQuerier
{
  Task<SpeciesModel?> ReadAsync(Guid id, CancellationToken cancellationToken = default);
  Task<IReadOnlyCollection<SpeciesModel>> ReadAsync(string slug, CancellationToken cancellationToken = default);

  Task<SearchResults<SpeciesModel>> SearchAsync(SearchSpeciesPayload payload, CancellationToken cancellationToken = default);
}
