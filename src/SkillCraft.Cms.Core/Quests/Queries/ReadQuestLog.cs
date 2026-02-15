using Krakenar.Contracts;
using Logitar.CQRS;
using SkillCraft.Cms.Core.Quests.Models;

namespace SkillCraft.Cms.Core.Quests.Queries;

internal record ReadQuestLogQuery(Guid? Id, string? Slug) : IQuery<QuestLogModel?>;

internal class ReadQuestLogQueryHandler : IQueryHandler<ReadQuestLogQuery, QuestLogModel?>
{
  private readonly IQuestLogQuerier _questLogQuerier;

  public ReadQuestLogQueryHandler(IQuestLogQuerier questLogQuerier)
  {
    _questLogQuerier = questLogQuerier;
  }

  public async Task<QuestLogModel?> HandleAsync(ReadQuestLogQuery query, CancellationToken cancellationToken)
  {
    Dictionary<Guid, QuestLogModel> questLogs = new(capacity: 2);

    if (query.Id.HasValue)
    {
      QuestLogModel? questLog = await _questLogQuerier.ReadAsync(query.Id.Value, cancellationToken);
      if (questLog is not null)
      {
        questLogs[questLog.Id] = questLog;
      }
    }

    if (!string.IsNullOrWhiteSpace(query.Slug))
    {
      QuestLogModel? questLog = await _questLogQuerier.ReadAsync(query.Slug, cancellationToken);
      if (questLog is not null)
      {
        questLogs[questLog.Id] = questLog;
      }
    }

    if (questLogs.Count > 1)
    {
      throw TooManyResultsException<QuestLogModel>.ExpectedSingle(questLogs.Count);
    }

    return questLogs.Values.SingleOrDefault();
  }
}
