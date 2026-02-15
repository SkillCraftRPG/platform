using Krakenar.Contracts.Actors;
using Krakenar.Core.Actors;
using Krakenar.EntityFrameworkCore.Relational.KrakenarDb;
using Logitar.Data;
using Logitar.EventSourcing;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Cms.Core.Quests;
using SkillCraft.Cms.Core.Quests.Models;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Queriers;

internal class QuestLogQuerier : IQuestLogQuerier
{
  private readonly IActorService _actorService;
  private readonly DbSet<QuestLogEntity> _questLogs;

  public QuestLogQuerier(IActorService actorService, EncyclopediaContext encyclopedia)
  {
    _actorService = actorService;
    _questLogs = encyclopedia.QuestLogs;
  }

  public async Task<QuestLogModel?> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    QuestLogEntity? questLog = await _questLogs.AsNoTracking()
      .Where(x => x.Id == id && x.IsPublished)
      .Include(x => x.Quests).ThenInclude(x => x.QuestGroup)
      .SingleOrDefaultAsync(cancellationToken);
    return questLog is null ? null : await MapAsync(questLog, cancellationToken);
  }
  public async Task<QuestLogModel?> ReadAsync(string slug, CancellationToken cancellationToken)
  {
    string slugNormalized = Helper.Normalize(slug);
    QuestLogEntity? questLog = await _questLogs.AsNoTracking()
      .Where(x => x.SlugNormalized == slugNormalized && x.IsPublished)
      .Include(x => x.Quests).ThenInclude(x => x.QuestGroup)
      .SingleOrDefaultAsync(cancellationToken);
    return questLog is null ? null : await MapAsync(questLog, cancellationToken);
  }

  private async Task<QuestLogModel> MapAsync(QuestLogEntity questLog, CancellationToken cancellationToken)
  {
    return (await MapAsync([questLog], cancellationToken)).Single();
  }
  private async Task<IReadOnlyCollection<QuestLogModel>> MapAsync(IEnumerable<QuestLogEntity> questLogs, CancellationToken cancellationToken)
  {
    IEnumerable<ActorId> actorIds = questLogs.SelectMany(questLog => questLog.GetActorIds());
    IReadOnlyDictionary<ActorId, Actor> actors = await _actorService.FindAsync(actorIds, cancellationToken);
    EncyclopediaMapper mapper = new(actors);

    return questLogs.Select(mapper.ToQuestLog).ToList().AsReadOnly();
  }
}
