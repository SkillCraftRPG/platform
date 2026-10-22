namespace SkillCraft.Cms.Core.Specializations.Models;

public class SpecializationTree
{
  public List<SpecializationNode> Nodes { get; set; } = [];

  public SpecializationTree()
  {
  }

  public SpecializationTree(IEnumerable<SpecializationNode> nodes)
  {
    Nodes.AddRange(nodes);
  }
}
