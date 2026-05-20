using Krakenar.Contracts.Search;

namespace SkillCraft.Cms.Core.Maps.Models;

public record SearchMapsPayload : SearchPayload
{
  public Guid? ArticleId { get; set; }

  public new List<MapSortOption> Sort { get; set; } = [];
}
