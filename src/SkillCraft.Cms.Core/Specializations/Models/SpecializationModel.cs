using Krakenar.Contracts;

namespace SkillCraft.Cms.Core.Specializations.Models;

public class SpecializationModel : Aggregate
{
  public string Slug { get; set; } = string.Empty;
  public string Name { get; set; } = string.Empty;

  public int Tier { get; set; }

  public string? MetaDescription { get; set; }
  public string? Summary { get; set; }
  public string? HtmlContent { get; set; }

  public RequirementsModel Requirements { get; set; } = new();
  public OptionsModel Options { get; set; } = new();
  public DoctrineModel? Doctrine { get; set; }

  public override string ToString() => $"{Name} | {base.ToString()}";
}
