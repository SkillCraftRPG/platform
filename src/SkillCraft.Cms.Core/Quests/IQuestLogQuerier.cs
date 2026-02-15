using SkillCraft.Cms.Core.Quests.Models;

namespace SkillCraft.Cms.Core.Quests;

public interface IQuestLogQuerier
{
  Task<QuestLogModel?> ReadAsync(Guid id, CancellationToken cancellationToken = default);
  Task<QuestLogModel?> ReadAsync(string slug, CancellationToken cancellationToken = default);
}
