using Krakenar.Contracts.Search;
using Krakenar.Web.Models.Search;
using SkillCraft.Cms.Core.Customizations;
using SkillCraft.Cms.Core.Customizations.Models;

namespace SkillCraft.Cms.Models.Parameters;

public record SearchCustomizationsParameters : SearchParameters
{
  public CustomizationKind? Kind { get; set; }

  public virtual SearchCustomizationsPayload ToPayload()
  {
    SearchCustomizationsPayload payload = new()
    {
      Kind = Kind
    };
    Fill(payload);

    foreach (SortOption item in ((SearchPayload)payload).Sort)
    {
      if (Enum.TryParse(item.Field, out CustomizationSort field))
      {
        payload.Sort.Add(new CustomizationSortOption(field, item.IsDescending));
      }
    }

    return payload;
  }
}
