using Krakenar.Core.Contents.Events;
using Logitar.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Materialization;

internal record UnpublishSpecializationCommand(ContentLocaleUnpublished Event) : ICommand;

internal class UnpublishSpecializationCommandHandler : ICommandHandler<UnpublishSpecializationCommand, Unit>
{
  private readonly ILogger<UnpublishSpecializationCommandHandler> _logger;
  private readonly RulesContext _rules;

  public UnpublishSpecializationCommandHandler(ILogger<UnpublishSpecializationCommandHandler> logger, RulesContext rules)
  {
    _logger = logger;
    _rules = rules;
  }

  public async Task<Unit> HandleAsync(UnpublishSpecializationCommand command, CancellationToken cancellationToken)
  {
    ContentLocaleUnpublished @event = command.Event;
    string streamId = @event.StreamId.Value;
    SpecializationEntity? specialization = await _rules.Specializations.SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);
    if (specialization is null)
    {
      _logger.LogWarning("The specialization 'StreamId={StreamId}' was not found.", streamId);
    }
    else
    {
      specialization.Unpublish(@event);

      await _rules.SaveChangesAsync(cancellationToken);
      _logger.LogInformation("The specialization '{Specialization}' has been unpublished.", specialization);
    }

    return Unit.Value;
  }
}
