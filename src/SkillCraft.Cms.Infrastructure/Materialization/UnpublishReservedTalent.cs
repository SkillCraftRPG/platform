using Krakenar.Core.Contents.Events;
using Logitar.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Materialization;

internal record UnpublishReservedTalentCommand(ContentLocaleUnpublished Event) : ICommand;

internal class UnpublishReservedTalentCommandHandler : ICommandHandler<UnpublishReservedTalentCommand, Unit>
{
  private readonly ILogger<UnpublishReservedTalentCommandHandler> _logger;
  private readonly RulesContext _rules;

  public UnpublishReservedTalentCommandHandler(ILogger<UnpublishReservedTalentCommandHandler> logger, RulesContext rules)
  {
    _logger = logger;
    _rules = rules;
  }

  public async Task<Unit> HandleAsync(UnpublishReservedTalentCommand command, CancellationToken cancellationToken)
  {
    ContentLocaleUnpublished @event = command.Event;
    string streamId = @event.StreamId.Value;
    ReservedTalentEntity? reservedTalent = await _rules.ReservedTalents.SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);
    if (reservedTalent is null)
    {
      _logger.LogWarning("The reserved talent 'StreamId={StreamId}' was not found.", streamId);
    }
    else
    {
      reservedTalent.Unpublish(@event);

      await _rules.SaveChangesAsync(cancellationToken);
      _logger.LogInformation("The reserved talent '{ReservedTalent}' has been unpublished.", reservedTalent);
    }

    return Unit.Value;
  }
}
