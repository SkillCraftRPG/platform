using Krakenar.Contracts.Search;

namespace SkillCraft.Cms.Core.Lineages.Models;

public record SearchEthnicitiesPayload : SearchPayload
{
  public Guid SpeciesId { get; set; }
  public Guid? LanguageId { get; set; }
  public SizeCategory? SizeCategory { get; set; }

  public new List<LineageSortOption> Sort { get; set; } = [];
}
