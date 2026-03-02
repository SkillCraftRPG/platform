using Krakenar.Contracts.Search;
using Krakenar.Web.Models.Search;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Cms.Core.Spells.Models;

namespace SkillCraft.Cms.Models.Parameters;

public record SearchSpellsParameters : SearchParameters
{
  [FromQuery(Name = "slug")]
  public List<string> Slugs { get; set; } = [];

  [FromQuery(Name = "tier")]
  public List<int> Tiers { get; set; } = [];

  [FromQuery(Name = "category")]
  public Guid? CategoryId { get; set; }

  public virtual SearchSpellsPayload ToPayload()
  {
    SearchSpellsPayload payload = new()
    {
      CategoryId = CategoryId
    };
    payload.Slugs.AddRange(Slugs);
    payload.Tiers.AddRange(Tiers);
    Fill(payload);

    foreach (SortOption item in ((SearchPayload)payload).Sort)
    {
      if (Enum.TryParse(item.Field, out SpellSort field))
      {
        payload.Sort.Add(new SpellSortOption(field, item.IsDescending));
      }
    }

    return payload;
  }
}
