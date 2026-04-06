namespace SkillCraft.Cms.Core.Lineages.Models;

public class SpeciesModel : LineageBase
{
  public SpeciesCategoryModel Category { get; set; } = new();

  public SizeModel Size { get; set; } = new();
  public WeightModel Weight { get; set; } = new();
  public AgeModel Age { get; set; } = new();

  public List<EthnicityModel> Ethnicities { get; set; } = [];
}
