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

    List<ValidationFailure> failures = [];

    reservedTalent.Key = locale.UniqueName.Value;
    reservedTalent.Name = locale.DisplayName?.Value ?? locale.UniqueName.Value;

    await SetSpecializationAsync(reservedTalent, invariant, failures, cancellationToken);
    await SetDiscountedTalentsAsync(reservedTalent, invariant, failures, cancellationToken);
    await SetFeaturesAsync(reservedTalent, invariant, failures, cancellationToken);

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

  private async Task SetDiscountedTalentsAsync(ReservedTalentEntity reservedTalent, ContentLocale invariant, List<ValidationFailure> failures, CancellationToken cancellationToken)
  {
    IReadOnlyCollection<Guid> talentIds = invariant.GetRelatedContent(ReservedTalentDefinition.DiscountedTalents);
    Dictionary<Guid, TalentEntity> talents = talentIds.Count < 1
      ? []
      : await _rules.Talents.Where(x => talentIds.Contains(x.Id)).ToDictionaryAsync(x => x.Id, x => x, cancellationToken);

    foreach (ReservedTalentDiscountedTalentEntity discountedTalent in reservedTalent.DiscountedTalents)
    {
      if (!talents.ContainsKey(discountedTalent.TalentUid))
      {
        _rules.ReservedTalentDiscountedTalents.Remove(discountedTalent);
      }
    }

    HashSet<Guid> existingIds = reservedTalent.DiscountedTalents.Select(x => x.TalentUid).ToHashSet();
    foreach (Guid talentId in talentIds)
    {
      if (talents.TryGetValue(talentId, out TalentEntity? talent))
      {
        if (!existingIds.Contains(talentId))
        {
          reservedTalent.AddDiscountedTalent(talent);
        }
      }
      else
      {
        failures.Add(new ValidationFailure(nameof(ReservedTalentDefinition.DiscountedTalents), "'{PropertyName}' must reference existing entities.", talentId)
        {
          ErrorCode = ErrorCodes.EntityNotFound
        });
      }
    }
  }

  private async Task SetFeaturesAsync(ReservedTalentEntity reservedTalent, ContentLocale invariant, List<ValidationFailure> failures, CancellationToken cancellationToken)
  {
    IReadOnlyCollection<Guid> featureIds = invariant.GetRelatedContent(ReservedTalentDefinition.Features);
    Dictionary<Guid, FeatureEntity> features = featureIds.Count < 1
      ? []
      : await _rules.Features.Where(x => featureIds.Contains(x.Id)).ToDictionaryAsync(x => x.Id, x => x, cancellationToken);

    foreach (ReservedTalentFeatureEntity feature in reservedTalent.Features)
    {
      if (!features.ContainsKey(feature.FeatureUid))
      {
        _rules.ReservedTalentFeatures.Remove(feature);
      }
    }

    HashSet<Guid> existingIds = reservedTalent.Features.Select(x => x.FeatureUid).ToHashSet();
    foreach (Guid featureId in featureIds)
    {
      if (features.TryGetValue(featureId, out FeatureEntity? feature))
      {
        if (!existingIds.Contains(featureId))
        {
          reservedTalent.AddFeature(feature);
        }
      }
      else
      {
        failures.Add(new ValidationFailure(nameof(ReservedTalentDefinition.Features), "'{PropertyName}' must reference existing entities.", featureId)
        {
          ErrorCode = ErrorCodes.EntityNotFound
        });
      }
    }
  }

  private async Task SetSpecializationAsync(ReservedTalentEntity reservedTalent, ContentLocale invariant, List<ValidationFailure> failures, CancellationToken cancellationToken)
  {
    IReadOnlyCollection<Guid> specializationIds = invariant.GetRelatedContent(ReservedTalentDefinition.Specialization);
    if (specializationIds.Count == 1)
    {
      Guid specializationId = specializationIds.Single();
      SpecializationEntity? specialization = await _rules.Specializations.SingleOrDefaultAsync(x => x.Id == specializationId, cancellationToken);
      if (specialization is null)
      {
        failures.Add(new ValidationFailure(nameof(ReservedTalentDefinition.Specialization), "'{PropertyName}' must reference an existing entity.", specializationId)
        {
          ErrorCode = ErrorCodes.EntityNotFound
        });
      }
      else
      {
        reservedTalent.SetSpecialization(specialization);
      }
    }
    else
    {
      failures.Add(new ValidationFailure(nameof(ReservedTalentDefinition.Specialization), "'{PropertyName}' must contain exactly one element.", specializationIds)
      {
        ErrorCode = specializationIds.Count < 1 ? ErrorCodes.EmptyValue : ErrorCodes.TooManyValues
      });
    }
  }
}

// TODO(fpion): Specialization Configurations (5)
// TODO(fpion): Migration
// TODO(fpion): Reserved Talent → Exclusive Talent? Doctrine?
