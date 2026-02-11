using Krakenar.Contracts.Search;

namespace SkillCraft.Cms.Core.Lineages.Models;

public record SearchSpeciesPayload : SearchPayload
{
  public Guid? LanguageId { get; set; }
  public SizeCategory? SizeCategory { get; set; }

  public new List<LineageSortOption> Sort { get; set; } = [];
}
