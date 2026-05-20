using Krakenar.Contracts.Search;
using Krakenar.Web.Models.Search;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Cms.Core.Maps.Models;

namespace SkillCraft.Cms.Models.Parameters;

public record SearchMapsParameters : SearchParameters
{
  [FromQuery(Name = "article")]
  public Guid? ArticleId { get; set; }

  public virtual SearchMapsPayload ToPayload()
  {
    SearchMapsPayload payload = new()
    {
      ArticleId = ArticleId
    };
    Fill(payload);

    foreach (SortOption item in ((SearchPayload)payload).Sort)
    {
      if (Enum.TryParse(item.Field, out MapSort field))
      {
        payload.Sort.Add(new MapSortOption(field, item.IsDescending));
      }
    }

    return payload;
  }
}
