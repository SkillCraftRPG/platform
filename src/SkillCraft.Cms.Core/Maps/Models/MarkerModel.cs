namespace SkillCraft.Cms.Core.Maps.Models;

public class MarkerModel
{
  public Guid Id { get; set; }

  public string Key { get; set; } = string.Empty;
  public string Title { get; set; } = string.Empty;

  public int X { get; set; }
  public int Y { get; set; }

  public string? HtmlContent { get; set; }

  public override bool Equals(object? obj) => obj is MarkerModel marker && marker.Id == Id;
  public override int GetHashCode() => Id.GetHashCode();
  public override string ToString() => $"{Title} (Id={Id})";
}
