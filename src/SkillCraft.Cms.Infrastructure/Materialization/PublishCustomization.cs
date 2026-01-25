using Krakenar.Core.Contents;
using Krakenar.Core.Contents.Events;
using Logitar.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillCraft.Cms.Core.Customizations;
using SkillCraft.Cms.Infrastructure.Contents;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Materialization;

internal record PublishCustomizationCommand(CustomizationKind Kind, ContentLocalePublished Event, ContentLocale Invariant, ContentLocale Locale) : ICommand;

internal class PublishCustomizationCommandHandler : ICommandHandler<PublishCustomizationCommand, Unit>
{
  private readonly ILogger<PublishCustomizationCommandHandler> _logger;
  private readonly RulesContext _rules;

  public PublishCustomizationCommandHandler(ILogger<PublishCustomizationCommandHandler> logger, RulesContext rules)
  {
    _logger = logger;
    _rules = rules;
  }

  public async Task<Unit> HandleAsync(PublishCustomizationCommand command, CancellationToken cancellationToken)
  {
    ContentLocalePublished @event = command.Event;
    ContentLocale invariant = command.Invariant;
    ContentLocale locale = command.Locale;

    string streamId = @event.StreamId.Value;
    CustomizationEntity? customization = await _rules.Customizations.SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);
    if (customization is null)
    {
      customization = new CustomizationEntity(command.Kind, command.Event);
      _rules.Customizations.Add(customization);
    }

    customization.Slug = locale.GetString(GetSlugId(customization));
    customization.Name = locale.DisplayName?.Value ?? locale.UniqueName.Value;

    customization.MetaDescription = locale.TryGetString(GetMetaDescriptionId(customization));
    customization.Summary = locale.TryGetString(GetSummaryId(customization));
    customization.HtmlContent = locale.TryGetString(GetHtmlContentId(customization));

    customization.Publish(@event);

    await _rules.SaveChangesAsync(cancellationToken);
    _logger.LogInformation("The customization '{Customization}' has been published.", customization);

    return Unit.Value;
  }

  private static Guid GetHtmlContentId(CustomizationEntity customization) => customization.Kind switch
  {
    CustomizationKind.Disability => DisabilityDefinition.HtmlContent,
    CustomizationKind.Gift => GiftDefinition.HtmlContent,
    _ => throw new NotSupportedException($"The customization kind '{customization.Kind}' is not supported."),
  };
  private static Guid GetMetaDescriptionId(CustomizationEntity customization) => customization.Kind switch
  {
    CustomizationKind.Disability => DisabilityDefinition.MetaDescription,
    CustomizationKind.Gift => GiftDefinition.MetaDescription,
    _ => throw new NotSupportedException($"The customization kind '{customization.Kind}' is not supported."),
  };
  private static Guid GetSlugId(CustomizationEntity customization) => customization.Kind switch
  {
    CustomizationKind.Disability => DisabilityDefinition.Slug,
    CustomizationKind.Gift => GiftDefinition.Slug,
    _ => throw new NotSupportedException($"The customization kind '{customization.Kind}' is not supported."),
  };
  private static Guid GetSummaryId(CustomizationEntity customization) => customization.Kind switch
  {
    CustomizationKind.Disability => DisabilityDefinition.Summary,
    CustomizationKind.Gift => GiftDefinition.Summary,
    _ => throw new NotSupportedException($"The customization kind '{customization.Kind}' is not supported."),
  };
}
