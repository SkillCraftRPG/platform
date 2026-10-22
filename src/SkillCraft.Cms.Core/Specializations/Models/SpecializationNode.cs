namespace SkillCraft.Cms.Core.Specializations.Models;

public class SpecializationNode
{
  public Guid Id { get; set; }
  public List<Guid> Parents { get; set; } = [];
  public List<Guid> Children { get; set; } = [];

  public int Tier { get; set; }
  public string Slug { get; set; } = string.Empty;
  public string Name { get; set; } = string.Empty;

  public string? Doctrine { get; set; }
  public string? Summary { get; set; }

  public override bool Equals(object? obj) => obj is SpecializationNode specialization && specialization.Id == Id;
  public override int GetHashCode() => Id.GetHashCode();
  public override string ToString() => $"{Name} (Id={Id})";
}
