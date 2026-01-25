using Krakenar.Core.Contents.Events;
using Logitar.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Materialization;

internal record UnpublishAttributeCommand(ContentLocaleUnpublished Event) : ICommand;

internal class UnpublishAttributeCommandHandler : ICommandHandler<UnpublishAttributeCommand, Unit>
{
  private readonly ILogger<UnpublishAttributeCommandHandler> _logger;
  private readonly RulesContext _rules;

  public UnpublishAttributeCommandHandler(ILogger<UnpublishAttributeCommandHandler> logger, RulesContext rules)
  {
    _logger = logger;
    _rules = rules;
  }

  public async Task<Unit> HandleAsync(UnpublishAttributeCommand command, CancellationToken cancellationToken)
  {
    ContentLocaleUnpublished @event = command.Event;
    string streamId = @event.StreamId.Value;
    AttributeEntity? attribute = await _rules.Attributes.SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);
    if (attribute is null)
    {
      _logger.LogWarning("The attribute 'StreamId={StreamId}' was not found.", streamId);
    }
    else
    {
      attribute.Unpublish(@event);

      await _rules.SaveChangesAsync(cancellationToken);
      _logger.LogInformation("The attribute '{Attribute}' has been unpublished.", attribute);
    }

    return Unit.Value;
  }
}
