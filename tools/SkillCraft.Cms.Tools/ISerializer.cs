namespace SkillCraft.Cms.Tools;

public interface ISerializer
{
  T? Deserialize<T>(string value);
  string Serialize<T>(T value);
}
