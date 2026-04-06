using Krakenar.Core.Contents;
using Krakenar.Core.Contents.Events;
using Logitar.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillCraft.Cms.Infrastructure.Contents;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Materialization;

internal record PublishSpeciesCategoryCommand(ContentLocalePublished Event, ContentLocale Invariant, ContentLocale Locale) : ICommand;

internal class PublishSpeciesCategoryCommandHandler : ICommandHandler<PublishSpeciesCategoryCommand, Unit>
{
  private readonly ILogger<PublishSpeciesCategoryCommandHandler> _logger;
  private readonly RulesContext _rules;

  public PublishSpeciesCategoryCommandHandler(ILogger<PublishSpeciesCategoryCommandHandler> logger, RulesContext rules)
  {
    _logger = logger;
    _rules = rules;
  }

  public async Task<Unit> HandleAsync(PublishSpeciesCategoryCommand command, CancellationToken cancellationToken)
  {
    ContentLocalePublished @event = command.Event;
    ContentLocale invariant = command.Invariant;
    ContentLocale locale = command.Locale;

    string streamId = @event.StreamId.Value;
    SpeciesCategoryEntity? speciesCategory = await _rules.SpeciesCategories.SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);
    if (speciesCategory is null)
    {
      speciesCategory = new SpeciesCategoryEntity(command.Event);
      _rules.SpeciesCategories.Add(speciesCategory);
    }

    speciesCategory.Key = locale.UniqueName.Value;
    speciesCategory.Name = locale.DisplayName?.Value ?? locale.UniqueName.Value;

    speciesCategory.Order = (int)invariant.GetNumber(SpeciesCategoryDefinition.Order);
    speciesCategory.Columns = (int)invariant.GetNumber(SpeciesCategoryDefinition.Columns);

    speciesCategory.HtmlContent = locale.TryGetString(SpeciesCategoryDefinition.HtmlContent);

    speciesCategory.Publish(@event);

    await _rules.SaveChangesAsync(cancellationToken);
    _logger.LogInformation("The species category '{SpeciesCategory}' has been published.", speciesCategory);

    return Unit.Value;
  }
}
