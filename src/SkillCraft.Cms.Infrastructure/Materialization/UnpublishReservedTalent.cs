using Krakenar.Core.Contents.Events;
using Logitar.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Materialization;

internal record UnpublishExclusiveTalentCommand(ContentLocaleUnpublished Event) : ICommand;

internal class UnpublishExclusiveTalentCommandHandler : ICommandHandler<UnpublishExclusiveTalentCommand, Unit>
{
  private readonly ILogger<UnpublishExclusiveTalentCommandHandler> _logger;
  private readonly RulesContext _rules;

  public UnpublishExclusiveTalentCommandHandler(ILogger<UnpublishExclusiveTalentCommandHandler> logger, RulesContext rules)
  {
    _logger = logger;
    _rules = rules;
  }

  public async Task<Unit> HandleAsync(UnpublishExclusiveTalentCommand command, CancellationToken cancellationToken)
  {
    ContentLocaleUnpublished @event = command.Event;
    string streamId = @event.StreamId.Value;
    ExclusiveTalentEntity? exclusiveTalent = await _rules.ExclusivedTalents.SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);
    if (exclusiveTalent is null)
    {
      _logger.LogWarning("The exclusive talent 'StreamId={StreamId}' was not found.", streamId);
    }
    else
    {
      exclusiveTalent.Unpublish(@event);

      await _rules.SaveChangesAsync(cancellationToken);
      _logger.LogInformation("The exclusive talent '{ExclusiveTalent}' has been unpublished.", exclusiveTalent);
    }

    return Unit.Value;
  }
}
