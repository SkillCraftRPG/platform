using Krakenar.Contracts;

namespace SkillCraft.Cms.Core.Maps.Models;

public class MapModel : Aggregate
{
  public string Key { get; set; } = string.Empty;
  public string Title { get; set; } = string.Empty;

  public int Width { get; set; }
  public int Height { get; set; }
  public string Source { get; set; } = string.Empty;

  public List<MarkerModel> Markers { get; set; } = [];

  public override string ToString() => $"{Title} | {base.ToString()}";
}
