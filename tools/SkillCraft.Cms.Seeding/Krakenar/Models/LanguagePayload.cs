using Krakenar.Contracts.Localization;

namespace SkillCraft.Cms.Seeding.Krakenar.Models;

internal record LanguagePayload : CreateOrReplaceLanguagePayload
{
  public bool IsDefault { get; set; }
}
