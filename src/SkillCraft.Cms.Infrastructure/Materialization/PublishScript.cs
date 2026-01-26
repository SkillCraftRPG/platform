using Krakenar.Core.Contents;
using Krakenar.Core.Contents.Events;
using Logitar.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillCraft.Cms.Infrastructure.Contents;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Materialization;

internal record PublishScriptCommand(ContentLocalePublished Event, ContentLocale Invariant, ContentLocale Locale) : ICommand;

internal class PublishScriptCommandHandler : ICommandHandler<PublishScriptCommand, Unit>
{
  private readonly ILogger<PublishScriptCommandHandler> _logger;
  private readonly RulesContext _rules;

  public PublishScriptCommandHandler(ILogger<PublishScriptCommandHandler> logger, RulesContext rules)
  {
    _logger = logger;
    _rules = rules;
  }

  public async Task<Unit> HandleAsync(PublishScriptCommand command, CancellationToken cancellationToken)
  {
    ContentLocalePublished @event = command.Event;
    ContentLocale invariant = command.Invariant;
    ContentLocale locale = command.Locale;

    string streamId = @event.StreamId.Value;
    ScriptEntity? script = await _rules.Scripts.SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);
    if (script is null)
    {
      script = new ScriptEntity(command.Event);
      _rules.Scripts.Add(script);
    }

    script.Slug = locale.GetString(ScriptDefinition.Slug);
    script.Name = locale.DisplayName?.Value ?? locale.UniqueName.Value;

    script.MetaDescription = locale.TryGetString(ScriptDefinition.MetaDescription);
    script.Summary = locale.TryGetString(ScriptDefinition.Summary);
    script.HtmlContent = locale.TryGetString(ScriptDefinition.HtmlContent);

    script.Publish(@event);

    await _rules.SaveChangesAsync(cancellationToken);
    _logger.LogInformation("The script '{Script}' has been published.", script);

    return Unit.Value;
  }
}
