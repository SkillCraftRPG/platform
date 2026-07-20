using Krakenar.Contracts.Dictionaries;

namespace SkillCraft.Cms.Seeding.Krakenar.Models;

internal record DictionaryPayload : CreateOrReplaceDictionaryPayload
{
  public Guid Id { get; set; }
}
