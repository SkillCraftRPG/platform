using Krakenar.Core.Contents.Events;
using Logitar.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Materialization;

internal record UnpublishSpellCommand(ContentLocaleUnpublished Event) : ICommand;

internal class UnpublishSpellCommandHandler : ICommandHandler<UnpublishSpellCommand, Unit>
{
  private readonly ILogger<UnpublishSpellCommandHandler> _logger;
  private readonly RulesContext _rules;

  public UnpublishSpellCommandHandler(ILogger<UnpublishSpellCommandHandler> logger, RulesContext rules)
  {
    _logger = logger;
    _rules = rules;
  }

  public async Task<Unit> HandleAsync(UnpublishSpellCommand command, CancellationToken cancellationToken)
  {
    ContentLocaleUnpublished @event = command.Event;
    string streamId = @event.StreamId.Value;
    SpellEntity? spell = await _rules.Spells.SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);
    if (spell is null)
    {
      _logger.LogWarning("The spell 'StreamId={StreamId}' was not found.", streamId);
    }
    else
    {
      spell.Unpublish(@event);

      await _rules.SaveChangesAsync(cancellationToken);
      _logger.LogInformation("The spell '{Spell}' has been unpublished.", spell);
    }

    return Unit.Value;
  }
}
