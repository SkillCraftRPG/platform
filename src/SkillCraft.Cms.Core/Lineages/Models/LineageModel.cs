namespace SkillCraft.Cms.Core.Lineages.Models;

public class LineageModel : LineageBase
{
  public LineageModel? Parent { get; set; }
  public List<LineageModel> Children { get; set; } = [];
}
