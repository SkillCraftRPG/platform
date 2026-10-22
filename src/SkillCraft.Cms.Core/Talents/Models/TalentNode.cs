namespace SkillCraft.Cms.Core.Talents.Models;

public class TalentNode
{
  public Guid Id { get; set; }
  public Guid? Parent { get; set; }
  public List<Guid> Children { get; set; } = [];

  public int Tier { get; set; }
  public string Slug { get; set; } = string.Empty;
  public string Name { get; set; } = string.Empty;

  public bool AllowMultiplePurchases { get; set; }
  public string? Skill { get; set; }
  public string? Summary { get; set; }

  public override bool Equals(object? obj) => obj is TalentNode talent && talent.Id == Id;
  public override int GetHashCode() => Id.GetHashCode();
  public override string ToString() => $"{Name} (Id={Id})";
}
