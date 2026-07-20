using Krakenar.Contracts.Senders;

namespace SkillCraft.Cms.Seeding.Krakenar.Models;

internal record SenderPayload : CreateOrReplaceSenderPayload
{
  public Guid Id { get; set; }
}
