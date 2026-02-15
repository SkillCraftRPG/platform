using Krakenar.Core.Contents;
using Krakenar.Core.Contents.Events;
using Logitar.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Materialization;

internal record PublishQuestGroupCommand(ContentLocalePublished Event, ContentLocale Invariant, ContentLocale Locale) : ICommand;

internal class PublishQuestGroupCommandHandler : ICommandHandler<PublishQuestGroupCommand, Unit>
{
  private readonly EncyclopediaContext _encyclopedia;
  private readonly ILogger<PublishQuestGroupCommandHandler> _logger;

  public PublishQuestGroupCommandHandler(EncyclopediaContext encyclopedia, ILogger<PublishQuestGroupCommandHandler> logger)
  {
    _encyclopedia = encyclopedia;
    _logger = logger;
  }

  public async Task<Unit> HandleAsync(PublishQuestGroupCommand command, CancellationToken cancellationToken)
  {
    ContentLocalePublished @event = command.Event;
    ContentLocale invariant = command.Invariant;
    ContentLocale locale = command.Locale;

    string streamId = @event.StreamId.Value;
    QuestGroupEntity? questGroup = await _encyclopedia.QuestGroups.SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);
    if (questGroup is null)
    {
      questGroup = new QuestGroupEntity(command.Event);
      _encyclopedia.QuestGroups.Add(questGroup);
    }

    questGroup.Name = locale.DisplayName?.Value ?? locale.UniqueName.Value;

    questGroup.Publish(@event);

    await _encyclopedia.SaveChangesAsync(cancellationToken);
    _logger.LogInformation("The quest group '{QuestGroup}' has been published.", questGroup);

    return Unit.Value;
  }
}
