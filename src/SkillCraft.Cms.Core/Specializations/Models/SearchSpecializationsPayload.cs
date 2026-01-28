using Krakenar.Contracts.Search;

namespace SkillCraft.Cms.Core.Specializations.Models;

public record SearchSpecializationsPayload : SearchPayload
{
  public List<string> Slugs { get; set; } = [];
  public List<int> Tiers { get; set; } = [];

  public new List<SpecializationSortOption> Sort { get; set; } = [];
}
