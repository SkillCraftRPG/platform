using Krakenar.Core.Contents.Events;
using Logitar.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Materialization;

internal record UnpublishScriptCommand(ContentLocaleUnpublished Event) : ICommand;

internal class UnpublishScriptCommandHandler : ICommandHandler<UnpublishScriptCommand, Unit>
{
  private readonly ILogger<UnpublishScriptCommandHandler> _logger;
  private readonly RulesContext _rules;

  public UnpublishScriptCommandHandler(ILogger<UnpublishScriptCommandHandler> logger, RulesContext rules)
  {
    _logger = logger;
    _rules = rules;
  }

  public async Task<Unit> HandleAsync(UnpublishScriptCommand command, CancellationToken cancellationToken)
  {
    ContentLocaleUnpublished @event = command.Event;
    string streamId = @event.StreamId.Value;
    ScriptEntity? script = await _rules.Scripts.SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);
    if (script is null)
    {
      _logger.LogWarning("The script 'StreamId={StreamId}' was not found.", streamId);
    }
    else
    {
      script.Unpublish(@event);

      await _rules.SaveChangesAsync(cancellationToken);
      _logger.LogInformation("The script '{Script}' has been unpublished.", script);
    }

    return Unit.Value;
  }
}
