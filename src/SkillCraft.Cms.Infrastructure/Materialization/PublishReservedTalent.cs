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

internal record PublishReservedTalentCommand(ContentLocalePublished Event, ContentLocale Invariant, ContentLocale Locale) : ICommand;

internal class PublishReservedTalentCommandHandler : ICommandHandler<PublishReservedTalentCommand, Unit>
{
  private readonly ILogger<PublishReservedTalentCommandHandler> _logger;
  private readonly RulesContext _rules;

  public PublishReservedTalentCommandHandler(ILogger<PublishReservedTalentCommandHandler> logger, RulesContext rules)
  {
    _logger = logger;
    _rules = rules;
  }

  public async Task<Unit> HandleAsync(PublishReservedTalentCommand command, CancellationToken cancellationToken)
  {
    ContentLocalePublished @event = command.Event;
    ContentLocale invariant = command.Invariant;
    ContentLocale locale = command.Locale;

    string streamId = @event.StreamId.Value;
    ReservedTalentEntity? reservedTalent = await _rules.ReservedTalents
      .Include(x => x.DiscountedTalents)
      .Include(x => x.Features)
      .SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);
    if (reservedTalent is null)
    {
      reservedTalent = new ReservedTalentEntity(command.Event);
      _rules.ReservedTalents.Add(reservedTalent);
    }

    List<ValidationFailure> failures = new(capacity: -1); // TODO(fpion): review

    reservedTalent.Key = locale.UniqueName.Value;
    reservedTalent.Name = locale.DisplayName?.Value ?? locale.UniqueName.Value;

    // TODO(fpion): Specialization
    // TODO(fpion): DiscountedTalents
    // TODO(fpion): Features

    reservedTalent.HtmlContent = locale.TryGetString(ReservedTalentDefinition.HtmlContent);

    reservedTalent.Publish(@event);

    if (failures.Count > 0)
    {
      _rules.ChangeTracker.Clear();
      throw new ValidationException(failures);
    }

    await _rules.SaveChangesAsync(cancellationToken);
    _logger.LogInformation("The reserved talent '{ReservedTalent}' has been published.", reservedTalent);

    return Unit.Value;
  }
}

// TODO(fpion): Specialization Configurations (5)
// TODO(fpion): Migration
// TODO(fpion): Reserved Talent → Exclusive Talent?
