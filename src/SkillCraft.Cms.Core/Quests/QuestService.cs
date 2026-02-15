using Logitar.CQRS;
using Microsoft.Extensions.DependencyInjection;
using SkillCraft.Cms.Core.Quests.Models;
using SkillCraft.Cms.Core.Quests.Queries;

namespace SkillCraft.Cms.Core.Quests;

public interface IQuestService
{
  Task<QuestLogModel?> ReadLogAsync(Guid? id = null, string? slug = null, CancellationToken cancellationToken = default);
}

internal class QuestService : IQuestService
{
  public static void Register(IServiceCollection services)
  {
    services.AddTransient<IQuestService, QuestService>();
    services.AddTransient<IQueryHandler<ReadQuestLogQuery, QuestLogModel?>, ReadQuestLogQueryHandler>();
  }

  private readonly IQueryBus _queryBus;

  public QuestService(IQueryBus queryBus)
  {
    _queryBus = queryBus;
  }

  public async Task<QuestLogModel?> ReadLogAsync(Guid? id, string? slug, CancellationToken cancellationToken)
  {
    ReadQuestLogQuery query = new(id, slug);
    return await _queryBus.ExecuteAsync(query, cancellationToken);
  }
}
