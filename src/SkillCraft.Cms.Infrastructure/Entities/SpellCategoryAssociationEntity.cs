namespace SkillCraft.Cms.Infrastructure.Entities;

internal class SpellCategoryAssociationEntity
{
  public SpellEntity? Spell { get; private set; }
  public int SpellId { get; private set; }
  public Guid SpellUid { get; private set; }

  public SpellCategoryEntity? SpellCategory { get; private set; }
  public int SpellCategoryId { get; private set; }
  public Guid SpellCategoryUid { get; private set; }

  public SpellCategoryAssociationEntity(SpellEntity spell, SpellCategoryEntity category)
  {
    Spell = spell;
    SpellId = spell.SpellId;
    SpellUid = spell.Id;

    SpellCategory = category;
    SpellCategoryId = category.SpellCategoryId;
    SpellCategoryUid = category.Id;
  }

  private SpellCategoryAssociationEntity()
  {
  }

  public override bool Equals(object? obj) => obj is SpellCategoryAssociationEntity entity && entity.SpellId == SpellId && entity.SpellCategoryId == SpellCategoryId;
  public override int GetHashCode() => HashCode.Combine(SpellId, SpellCategoryId);
  public override string ToString() => $"{GetType()} (SpellId={SpellId}, SpellCategoryId={SpellCategoryId})";
}
