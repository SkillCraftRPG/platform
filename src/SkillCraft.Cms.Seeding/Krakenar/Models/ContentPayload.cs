namespace SkillCraft.Cms.Seeding.Krakenar.Models;

internal record ContentPayload
{
  public Guid Id { get; set; }

  public ContentLocalePayload Invariant { get; set; } = new();
  public Dictionary<string, ContentLocalePayload> Locales { get; set; } = [];
}
