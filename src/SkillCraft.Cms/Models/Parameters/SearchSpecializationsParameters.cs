using Krakenar.Contracts.Search;
using Krakenar.Web.Models.Search;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Cms.Core.Specializations.Models;

namespace SkillCraft.Cms.Models.Parameters;

public record SearchSpecializationsParameters : SearchParameters
{
  [FromQuery(Name = "slug")]
  public List<string> Slugs { get; set; } = [];

  [FromQuery(Name = "tier")]
  public List<int> Tiers { get; set; } = [];

  public virtual SearchSpecializationsPayload ToPayload()
  {
    SearchSpecializationsPayload payload = new();
    payload.Slugs.AddRange(Slugs);
    payload.Tiers.AddRange(Tiers);
    Fill(payload);

    foreach (SortOption item in ((SearchPayload)payload).Sort)
    {
      if (Enum.TryParse(item.Field, out SpecializationSort field))
      {
        payload.Sort.Add(new SpecializationSortOption(field, item.IsDescending));
      }
    }

    return payload;
  }
}
