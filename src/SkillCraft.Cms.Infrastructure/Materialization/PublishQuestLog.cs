using Krakenar.Core.Contents;
using Krakenar.Core.Contents.Events;
using Logitar.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillCraft.Cms.Infrastructure.Contents;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Materialization;

internal record PublishQuestLogCommand(ContentLocalePublished Event, ContentLocale Invariant, ContentLocale Locale) : ICommand;

internal class PublishQuestLogCommandHandler : ICommandHandler<PublishQuestLogCommand, Unit>
{
  private readonly EncyclopediaContext _encyclopedia;
  private readonly ILogger<PublishQuestLogCommandHandler> _logger;

  public PublishQuestLogCommandHandler(EncyclopediaContext encyclopedia, ILogger<PublishQuestLogCommandHandler> logger)
  {
    _encyclopedia = encyclopedia;
    _logger = logger;
  }

  public async Task<Unit> HandleAsync(PublishQuestLogCommand command, CancellationToken cancellationToken)
  {
    ContentLocalePublished @event = command.Event;
    ContentLocale invariant = command.Invariant;
    ContentLocale locale = command.Locale;

    string streamId = @event.StreamId.Value;
    QuestLogEntity? questLog = await _encyclopedia.QuestLogs.SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);
    if (questLog is null)
    {
      questLog = new QuestLogEntity(command.Event);
      _encyclopedia.QuestLogs.Add(questLog);
    }

    questLog.Slug = locale.GetString(QuestLogDefinition.Slug);
    questLog.Title = locale.DisplayName?.Value ?? locale.UniqueName.Value;

    questLog.MetaDescription = locale.TryGetString(QuestLogDefinition.MetaDescription);
    questLog.HtmlContent = locale.TryGetString(QuestLogDefinition.HtmlContent);

    questLog.Publish(@event);

    await _encyclopedia.SaveChangesAsync(cancellationToken);
    _logger.LogInformation("The quest log '{QuestLog}' has been published.", questLog);

    return Unit.Value;
  }
}
