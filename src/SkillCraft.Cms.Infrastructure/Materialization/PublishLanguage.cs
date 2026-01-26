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

internal record PublishLanguageCommand(ContentLocalePublished Event, ContentLocale Invariant, ContentLocale Locale) : ICommand;

internal class PublishLanguageCommandHandler : ICommandHandler<PublishLanguageCommand, Unit>
{
  private readonly ILogger<PublishLanguageCommandHandler> _logger;
  private readonly RulesContext _rules;

  public PublishLanguageCommandHandler(ILogger<PublishLanguageCommandHandler> logger, RulesContext rules)
  {
    _logger = logger;
    _rules = rules;
  }

  public async Task<Unit> HandleAsync(PublishLanguageCommand command, CancellationToken cancellationToken)
  {
    ContentLocalePublished @event = command.Event;
    ContentLocale invariant = command.Invariant;
    ContentLocale locale = command.Locale;

    string streamId = @event.StreamId.Value;
    LanguageEntity? language = await _rules.Languages.SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);
    if (language is null)
    {
      language = new LanguageEntity(command.Event);
      _rules.Languages.Add(language);
    }

    List<ValidationFailure> failures = new(capacity: 1);

    language.Slug = locale.GetString(LanguageDefinition.Slug);
    language.Name = locale.DisplayName?.Value ?? locale.UniqueName.Value;

    await SetScriptAsync(language, invariant, failures, cancellationToken);
    language.TypicalSpeakers = locale.TryGetString(LanguageDefinition.TypicalSpeakers);

    language.MetaDescription = locale.TryGetString(LanguageDefinition.MetaDescription);
    language.Summary = locale.TryGetString(LanguageDefinition.Summary);
    language.HtmlContent = locale.TryGetString(LanguageDefinition.HtmlContent);

    language.Publish(@event);

    if (failures.Count > 0)
    {
      _rules.ChangeTracker.Clear();
      throw new ValidationException(failures);
    }

    await _rules.SaveChangesAsync(cancellationToken);
    _logger.LogInformation("The language '{Language}' has been published.", language);

    return Unit.Value;
  }

  private async Task SetScriptAsync(LanguageEntity language, ContentLocale invariant, List<ValidationFailure> failures, CancellationToken cancellationToken)
  {
    IReadOnlyCollection<Guid> scriptIds = invariant.GetRelatedContent(LanguageDefinition.Script);
    if (scriptIds.Count < 1)
    {
      language.SetScript(null);
    }
    else if (scriptIds.Count > 1)
    {
      failures.Add(new ValidationFailure(nameof(LanguageDefinition.Script), "'{PropertyName}' must contain at most one element.", scriptIds)
      {
        ErrorCode = ErrorCodes.TooManyValues
      });
    }
    else
    {
      Guid scriptId = scriptIds.Single();
      ScriptEntity? script = await _rules.Scripts.SingleOrDefaultAsync(x => x.Id == scriptId, cancellationToken);
      if (script is null)
      {
        failures.Add(new ValidationFailure(nameof(LanguageDefinition.Script), "'{PropertyName}' did not reference an existing entity.", scriptId)
        {
          ErrorCode = ErrorCodes.EntityNotFound
        });
      }
      else
      {
        language.SetScript(script);
      }
    }
  }
}
