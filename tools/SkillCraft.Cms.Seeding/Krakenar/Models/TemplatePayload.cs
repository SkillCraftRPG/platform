using Krakenar.Contracts.Templates;

namespace SkillCraft.Cms.Seeding.Krakenar.Models;

internal record TemplatePayload : CreateOrReplaceTemplatePayload
{
  public Guid Id { get; set; }
}
