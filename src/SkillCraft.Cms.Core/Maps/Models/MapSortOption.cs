using Krakenar.Contracts.Search;

namespace SkillCraft.Cms.Core.Maps.Models;

public record MapSortOption : SortOption
{
  public new MapSort Field
  {
    get => Enum.Parse<MapSort>(base.Field);
    set => base.Field = value.ToString();
  }

  public MapSortOption() : this(MapSort.Title)
  {
  }

  public MapSortOption(MapSort field, bool isDescending = false) : base(field.ToString(), isDescending)
  {
  }
}
