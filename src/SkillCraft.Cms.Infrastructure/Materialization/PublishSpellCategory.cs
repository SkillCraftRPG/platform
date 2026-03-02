using FluentValidation;
using FluentValidation.Results;
using Krakenar.Core.Contents;
using Krakenar.Core.Contents.Events;
using Logitar.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillCraft.Cms.Infrastructure.Contents;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Materialization;

internal record PublishSpellCategoryCommand(ContentLocalePublished Event, ContentLocale Invariant, ContentLocale Locale) : ICommand;

internal class PublishSpellCategoryCommandHandler : ICommandHandler<PublishSpellCategoryCommand, Unit>
{
  private readonly ILogger<PublishSpellCategoryCommandHandler> _logger;
  private readonly RulesContext _rules;

  public PublishSpellCategoryCommandHandler(ILogger<PublishSpellCategoryCommandHandler> logger, RulesContext rules)
  {
    _logger = logger;
    _rules = rules;
  }

  public async Task<Unit> HandleAsync(PublishSpellCategoryCommand command, CancellationToken cancellationToken)
  {
    ContentLocalePublished @event = command.Event;
    ContentLocale invariant = command.Invariant;
    ContentLocale locale = command.Locale;

    string streamId = @event.StreamId.Value;
    SpellCategoryEntity? spellCategory = await _rules.SpellCategories.SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);
    if (spellCategory is null)
    {
      spellCategory = new SpellCategoryEntity(command.Event);
      _rules.SpellCategories.Add(spellCategory);
    }

    List<ValidationFailure> failures = new(capacity: 1);

    spellCategory.Key = locale.UniqueName.Value;
    spellCategory.Name = locale.DisplayName?.Value ?? locale.UniqueName.Value;

    await SetParentAsync(spellCategory, invariant, failures, cancellationToken);

    spellCategory.Publish(@event);

    if (failures.Count > 0)
    {
      _rules.ChangeTracker.Clear();
      throw new ValidationException(failures);
    }

    await _rules.SaveChangesAsync(cancellationToken);
    _logger.LogInformation("The spell category '{SpellCategory}' has been published.", spellCategory);

    return Unit.Value;
  }

  private async Task SetParentAsync(SpellCategoryEntity spellCategory, ContentLocale invariant, List<ValidationFailure> failures, CancellationToken cancellationToken)
  {
    IReadOnlyCollection<Guid> parentIds = invariant.GetRelatedContent(SpellCategoryDefinition.Parent);
    if (parentIds.Count < 1)
    {
      spellCategory.SetParent(null);
    }
    else if (parentIds.Count > 1)
    {
      failures.Add(new ValidationFailure(nameof(SpellCategoryDefinition.Parent), "'{PropertyName}' must contain at most one element.", parentIds)
      {
        ErrorCode = ErrorCodes.TooManyValues
      });
    }
    else
    {
      Guid parentId = parentIds.Single();
      SpellCategoryEntity? parent = await _rules.SpellCategories.SingleOrDefaultAsync(x => x.Id == parentId, cancellationToken);
      if (parent is null)
      {
        failures.Add(new ValidationFailure(nameof(SpellCategoryDefinition.Parent), "'{PropertyName}' must reference an existing entity.", parentId)
        {
          ErrorCode = ErrorCodes.EntityNotFound
        });
      }
      else
      {
        spellCategory.SetParent(parent);
      }
    }
  }
}
