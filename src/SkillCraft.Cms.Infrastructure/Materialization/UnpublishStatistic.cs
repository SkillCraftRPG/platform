using Krakenar.Core.Contents.Events;
using Logitar.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Materialization;

internal record UnpublishStatisticCommand(ContentLocaleUnpublished Event) : ICommand;

internal class UnpublishStatisticCommandHandler : ICommandHandler<UnpublishStatisticCommand, Unit>
{
  private readonly ILogger<UnpublishStatisticCommandHandler> _logger;
  private readonly RulesContext _rules;

  public UnpublishStatisticCommandHandler(ILogger<UnpublishStatisticCommandHandler> logger, RulesContext rules)
  {
    _logger = logger;
    _rules = rules;
  }

  public async Task<Unit> HandleAsync(UnpublishStatisticCommand command, CancellationToken cancellationToken)
  {
    ContentLocaleUnpublished @event = command.Event;
    string streamId = @event.StreamId.Value;
    StatisticEntity? statistic = await _rules.Statistics.SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);
    if (statistic is null)
    {
      _logger.LogWarning("The statistic 'StreamId={StreamId}' was not found.", streamId);
    }
    else
    {
      statistic.Unpublish(@event);

      await _rules.SaveChangesAsync(cancellationToken);
      _logger.LogInformation("The statistic '{Statistic}' has been unpublished.", statistic);
    }

    return Unit.Value;
  }
}
