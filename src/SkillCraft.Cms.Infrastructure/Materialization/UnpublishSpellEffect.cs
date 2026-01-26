using Krakenar.Core.Contents.Events;
using Logitar.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Materialization;

internal record UnpublishSpellEffectCommand(ContentLocaleUnpublished Event) : ICommand;

internal class UnpublishSpellEffectCommandHandler : ICommandHandler<UnpublishSpellEffectCommand, Unit>
{
  private readonly ILogger<UnpublishSpellEffectCommandHandler> _logger;
  private readonly RulesContext _rules;

  public UnpublishSpellEffectCommandHandler(ILogger<UnpublishSpellEffectCommandHandler> logger, RulesContext rules)
  {
    _logger = logger;
    _rules = rules;
  }

  public async Task<Unit> HandleAsync(UnpublishSpellEffectCommand command, CancellationToken cancellationToken)
  {
    ContentLocaleUnpublished @event = command.Event;
    string streamId = @event.StreamId.Value;
    SpellEffectEntity? spellEffect = await _rules.SpellEffects.SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);
    if (spellEffect is null)
    {
      _logger.LogWarning("The spell effect 'StreamId={StreamId}' was not found.", streamId);
    }
    else
    {
      spellEffect.Unpublish(@event);

      await _rules.SaveChangesAsync(cancellationToken);
      _logger.LogInformation("The spell effect '{SpellEffect}' has been unpublished.", spellEffect);
    }

    return Unit.Value;
  }
}
