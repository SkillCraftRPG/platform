using FluentValidation;
using FluentValidation.Results;
using Krakenar.Core.Contents;
using Krakenar.Core.Contents.Events;
using Logitar.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillCraft.Cms.Infrastructure.Contents;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Materialization;

internal record PublishQuestCommand(ContentLocalePublished Event, ContentLocale Invariant, ContentLocale Locale) : ICommand;

internal class PublishQuestCommandHandler : ICommandHandler<PublishQuestCommand, Unit>
{
  private readonly EncyclopediaContext _encyclopedia;
  private readonly ILogger<PublishQuestCommandHandler> _logger;

  public PublishQuestCommandHandler(EncyclopediaContext encyclopedia, ILogger<PublishQuestCommandHandler> logger)
  {
    _encyclopedia = encyclopedia;
    _logger = logger;
  }

  public async Task<Unit> HandleAsync(PublishQuestCommand command, CancellationToken cancellationToken)
  {
    ContentLocalePublished @event = command.Event;
    ContentLocale invariant = command.Invariant;
    ContentLocale locale = command.Locale;

    string streamId = @event.StreamId.Value;
    QuestEntity? quest = await _encyclopedia.Quests.SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);
    if (quest is null)
    {
      quest = new QuestEntity(command.Event);
      _encyclopedia.Quests.Add(quest);
    }

    List<ValidationFailure> failures = new(capacity: 2);

    quest.Title = locale.DisplayName?.Value ?? locale.UniqueName.Value;

    await SetLogAsync(quest, invariant, failures, cancellationToken);
    await SetGroupAsync(quest, invariant, failures, cancellationToken);

    quest.GrantedLevels = (int)invariant.GetNumber(QuestDefinition.GrantedLevels);
    quest.ProgressRatio = invariant.GetNumber(QuestDefinition.ProgressRatio);

    quest.HtmlContent = locale.TryGetString(QuestDefinition.HtmlContent);
    quest.CompletedEntries = locale.TryGetString(QuestDefinition.CompletedEntries);
    quest.ActiveEntries = locale.TryGetString(QuestDefinition.ActiveEntries);

    quest.Publish(@event);

    if (failures.Count > 0)
    {
      _encyclopedia.ChangeTracker.Clear();
      throw new ValidationException(failures);
    }

    await _encyclopedia.SaveChangesAsync(cancellationToken);
    _logger.LogInformation("The quest '{Quest}' has been published.", quest);

    return Unit.Value;
  }

  private async Task SetLogAsync(QuestEntity quest, ContentLocale invariant, List<ValidationFailure> failures, CancellationToken cancellationToken)
  {
    IReadOnlyCollection<Guid> logIds = invariant.GetRelatedContent(QuestDefinition.QuestLog);
    if (logIds.Count == 1)
    {
      Guid logId = logIds.Single();
      QuestLogEntity? log = await _encyclopedia.QuestLogs.SingleOrDefaultAsync(x => x.Id == logId, cancellationToken);
      if (log is null)
      {
        failures.Add(new ValidationFailure(nameof(QuestDefinition.QuestLog), "'{PropertyName}' must reference an existing entity.", logId)
        {
          ErrorCode = ErrorCodes.EntityNotFound
        });
      }
      else
      {
        quest.SetQuestLog(log);
      }
    }
    else
    {
      failures.Add(new ValidationFailure(nameof(QuestDefinition.QuestLog), "'{PropertyName}' must contain exactly one element.", logIds)
      {
        ErrorCode = logIds.Count < 1 ? ErrorCodes.EmptyValue : ErrorCodes.TooManyValues
      });
    }
  }

  private async Task SetGroupAsync(QuestEntity quest, ContentLocale invariant, List<ValidationFailure> failures, CancellationToken cancellationToken)
  {
    IReadOnlyCollection<Guid> groupIds = invariant.GetRelatedContent(QuestDefinition.QuestGroup);
    if (groupIds.Count < 1)
    {
      quest.SetQuestGroup(null);
    }
    else if (groupIds.Count > 1)
    {
      failures.Add(new ValidationFailure(nameof(QuestDefinition.QuestGroup), "'{PropertyName}' must contain at most one element.", groupIds)
      {
        ErrorCode = ErrorCodes.TooManyValues
      });
    }
    else
    {
      Guid groupId = groupIds.Single();
      QuestGroupEntity? group = await _encyclopedia.QuestGroups.SingleOrDefaultAsync(x => x.Id == groupId, cancellationToken);
      if (group is null)
      {
        failures.Add(new ValidationFailure(nameof(QuestDefinition.QuestGroup), "'{PropertyName}' must reference an existing entity.", groupId)
        {
          ErrorCode = ErrorCodes.EntityNotFound
        });
      }
      else
      {
        quest.SetQuestGroup(group);
      }
    }
  }
}
