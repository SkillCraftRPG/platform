using Krakenar.Contracts.Search;

namespace SkillCraft.Cms.Core.Lineages.Models;

public record SearchEthnicitiesPayload : SearchPayload
{
  public string Species { get; set; } = string.Empty;
  public Guid? LanguageId { get; set; }
  public SizeCategory? SizeCategory { get; set; }

  public new List<LineageSortOption> Sort { get; set; } = [];
}
