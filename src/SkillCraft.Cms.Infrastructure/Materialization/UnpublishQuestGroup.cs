using Krakenar.Core.Contents.Events;
using Logitar.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Materialization;

internal record UnpublishQuestGroupCommand(ContentLocaleUnpublished Event) : ICommand;

internal class UnpublishQuestGroupCommandHandler : ICommandHandler<UnpublishQuestGroupCommand, Unit>
{
  private readonly EncyclopediaContext _encyclopedia;
  private readonly ILogger<UnpublishQuestGroupCommandHandler> _logger;

  public UnpublishQuestGroupCommandHandler(EncyclopediaContext encyclopedia, ILogger<UnpublishQuestGroupCommandHandler> logger)
  {
    _encyclopedia = encyclopedia;
    _logger = logger;
  }

  public async Task<Unit> HandleAsync(UnpublishQuestGroupCommand command, CancellationToken cancellationToken)
  {
    ContentLocaleUnpublished @event = command.Event;
    string streamId = @event.StreamId.Value;
    QuestGroupEntity? questGroup = await _encyclopedia.QuestGroups.SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);
    if (questGroup is null)
    {
      _logger.LogWarning("The quest group 'StreamId={StreamId}' was not found.", streamId);
    }
    else
    {
      questGroup.Unpublish(@event);

      await _encyclopedia.SaveChangesAsync(cancellationToken);
      _logger.LogInformation("The quest group '{QuestGroup}' has been unpublished.", questGroup);
    }

    return Unit.Value;
  }
}
