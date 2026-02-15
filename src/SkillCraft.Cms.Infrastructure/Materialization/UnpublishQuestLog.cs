using Krakenar.Core.Contents.Events;
using Logitar.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Materialization;

internal record UnpublishQuestLogCommand(ContentLocaleUnpublished Event) : ICommand;

internal class UnpublishQuestLogCommandHandler : ICommandHandler<UnpublishQuestLogCommand, Unit>
{
  private readonly EncyclopediaContext _encyclopedia;
  private readonly ILogger<UnpublishQuestLogCommandHandler> _logger;

  public UnpublishQuestLogCommandHandler(EncyclopediaContext encyclopedia, ILogger<UnpublishQuestLogCommandHandler> logger)
  {
    _encyclopedia = encyclopedia;
    _logger = logger;
  }

  public async Task<Unit> HandleAsync(UnpublishQuestLogCommand command, CancellationToken cancellationToken)
  {
    ContentLocaleUnpublished @event = command.Event;
    string streamId = @event.StreamId.Value;
    QuestLogEntity? questLog = await _encyclopedia.QuestLogs.SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);
    if (questLog is null)
    {
      _logger.LogWarning("The quest log 'StreamId={StreamId}' was not found.", streamId);
    }
    else
    {
      questLog.Unpublish(@event);

      await _encyclopedia.SaveChangesAsync(cancellationToken);
      _logger.LogInformation("The quest log '{QuestLog}' has been unpublished.", questLog);
    }

    return Unit.Value;
  }
}
