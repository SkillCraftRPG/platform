using Krakenar.Contracts.Realms;

namespace SkillCraft.Cms.Seeding.Krakenar.Models;

internal record RealmPayload : CreateOrReplaceRealmPayload
{
  public Guid Id { get; set; }
}
