using Krakenar.Contracts.Search;
using Krakenar.Web.Models.Search;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Cms.Core.Lineages;
using SkillCraft.Cms.Core.Lineages.Models;

namespace SkillCraft.Cms.Models.Parameters;

public record SearchSpeciesParameters : SearchParameters
{
  [FromQuery(Name = "language")]
  public Guid? LanguageId { get; set; }

  [FromQuery(Name = "size")]
  public SizeCategory? SizeCategory { get; set; }

  public virtual SearchSpeciesPayload ToPayload()
  {
    SearchSpeciesPayload payload = new()
    {
      LanguageId = LanguageId,
      SizeCategory = SizeCategory
    };
    Fill(payload);

    foreach (SortOption item in ((SearchPayload)payload).Sort)
    {
      if (Enum.TryParse(item.Field, out LineageSort field))
      {
        payload.Sort.Add(new LineageSortOption(field, item.IsDescending));
      }
    }

    return payload;
  }
}
