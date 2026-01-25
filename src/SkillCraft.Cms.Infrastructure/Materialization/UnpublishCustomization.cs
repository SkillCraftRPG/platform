using Krakenar.Core.Contents.Events;
using Logitar.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Materialization;

internal record UnpublishCustomizationCommand(ContentLocaleUnpublished Event) : ICommand;

internal class UnpublishCustomizationCommandHandler : ICommandHandler<UnpublishCustomizationCommand, Unit>
{
  private readonly ILogger<UnpublishCustomizationCommandHandler> _logger;
  private readonly RulesContext _rules;

  public UnpublishCustomizationCommandHandler(ILogger<UnpublishCustomizationCommandHandler> logger, RulesContext rules)
  {
    _logger = logger;
    _rules = rules;
  }

  public async Task<Unit> HandleAsync(UnpublishCustomizationCommand command, CancellationToken cancellationToken)
  {
    ContentLocaleUnpublished @event = command.Event;
    string streamId = @event.StreamId.Value;
    CustomizationEntity? customization = await _rules.Customizations.SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);
    if (customization is null)
    {
      _logger.LogWarning("The customization 'StreamId={StreamId}' was not found.", streamId);
    }
    else
    {
      customization.Unpublish(@event);

      await _rules.SaveChangesAsync(cancellationToken);
      _logger.LogInformation("The customization '{Customization}' has been unpublished.", customization);
    }

    return Unit.Value;
  }
}
