using Krakenar.Contracts.Search;
using SkillCraft.Cms.Core.Lineages.Models;

namespace SkillCraft.Cms.Core.Lineages;

public interface IEthnicityQuerier
{
  Task<EthnicityModel?> ReadAsync(Guid id, CancellationToken cancellationToken = default);

  Task<SearchResults<EthnicityModel>> SearchAsync(SearchEthnicitiesPayload payload, CancellationToken cancellationToken = default);
}
