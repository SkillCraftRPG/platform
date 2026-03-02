namespace SkillCraft.Cms.Core.Spells.Models;

public class SpellCategoryModel
{
  public Guid Id { get; set; }

  public string Key { get; set; } = string.Empty;
  public string Name { get; set; } = string.Empty;

  public SpellCategoryModel? Parent { get; set; }
  public List<SpellCategoryModel> Children { get; set; } = [];
}
