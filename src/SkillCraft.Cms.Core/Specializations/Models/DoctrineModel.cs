using SkillCraft.Cms.Core.Features.Models;
using SkillCraft.Cms.Core.Talents.Models;

namespace SkillCraft.Cms.Core.Specializations.Models;

public record DoctrineModel
{
  public string Name { get; set; }
  public List<string> Description { get; set; } = [];
  public List<TalentModel> DiscountedTalents { get; set; } = [];
  public List<FeatureModel> Features { get; set; } = [];

  public DoctrineModel() : this(string.Empty)
  {
  }

  public DoctrineModel(string name)
  {
    Name = name;
  }
}
