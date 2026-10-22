namespace SkillCraft.Cms.Core.Talents.Models;

public class TalentTree
{
  public List<TalentNode> Nodes { get; set; } = [];

  public TalentTree()
  {
  }

  public TalentTree(IEnumerable<TalentNode> nodes)
  {
    Nodes.AddRange(nodes);
  }
}
