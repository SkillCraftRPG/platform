using Krakenar.Core.Contents.Events;
using Logitar.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Materialization;

internal record UnpublishQuestCommand(ContentLocaleUnpublished Event) : ICommand;

internal class UnpublishQuestCommandHandler : ICommandHandler<UnpublishQuestCommand, Unit>
{
  private readonly EncyclopediaContext _encyclopedia;
  private readonly ILogger<UnpublishQuestCommandHandler> _logger;

  public UnpublishQuestCommandHandler(EncyclopediaContext encyclopedia, ILogger<UnpublishQuestCommandHandler> logger)
  {
    _encyclopedia = encyclopedia;
    _logger = logger;
  }

  public async Task<Unit> HandleAsync(UnpublishQuestCommand command, CancellationToken cancellationToken)
  {
    ContentLocaleUnpublished @event = command.Event;
    string streamId = @event.StreamId.Value;
    QuestEntity? quest = await _encyclopedia.Quests.SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);
    if (quest is null)
    {
      _logger.LogWarning("The quest 'StreamId={StreamId}' was not found.", streamId);
    }
    else
    {
      quest.Unpublish(@event);

      await _encyclopedia.SaveChangesAsync(cancellationToken);
      _logger.LogInformation("The quest '{Quest}' has been unpublished.", quest);
    }

    return Unit.Value;
  }
}
