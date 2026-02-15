namespace SkillCraft.Cms.Core.Quests.Models;

public class QuestGroupModel
{
  public Guid Id { get; set; }
  public string Name { get; set; }

  public QuestGroupModel() : this(Guid.Empty, string.Empty)
  {
  }

  public QuestGroupModel(Guid id, string name)
  {
    Id = id;
    Name = name;
  }

  public override bool Equals(object? obj) => obj is QuestGroupModel group && group.Id == Id;
  public override int GetHashCode() => Id.GetHashCode();
  public override string ToString() => $"{Name} (Id={Id})";
}
