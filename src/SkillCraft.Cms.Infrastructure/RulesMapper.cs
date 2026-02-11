using Krakenar.Contracts;
using Krakenar.Contracts.Actors;
using Logitar;
using Logitar.EventSourcing;
using SkillCraft.Cms.Core.Attributes.Models;
using SkillCraft.Cms.Core.Castes.Models;
using SkillCraft.Cms.Core.Customizations.Models;
using SkillCraft.Cms.Core.Educations.Models;
using SkillCraft.Cms.Core.Features.Models;
using SkillCraft.Cms.Core.Languages.Models;
using SkillCraft.Cms.Core.Lineages.Models;
using SkillCraft.Cms.Core.Scripts.Models;
using SkillCraft.Cms.Core.Skills.Models;
using SkillCraft.Cms.Core.Specializations.Models;
using SkillCraft.Cms.Core.Spells.Models;
using SkillCraft.Cms.Core.Statistics.Models;
using SkillCraft.Cms.Core.Talents.Models;
using SkillCraft.Cms.Infrastructure.Configurations;
using SkillCraft.Cms.Infrastructure.Entities;
using AggregateEntity = Krakenar.EntityFrameworkCore.Relational.Entities.Aggregate;

namespace SkillCraft.Cms.Infrastructure;

internal class RulesMapper
{
  private readonly Dictionary<ActorId, Actor> _actors = [];
  private readonly Actor _system = new();

  public RulesMapper()
  {
  }

  public RulesMapper(IReadOnlyDictionary<ActorId, Actor> actors)
  {
    foreach (KeyValuePair<ActorId, Actor> actor in actors)
    {
      _actors[actor.Key] = actor.Value;
    }
  }

  public AttributeModel ToAttribute(AttributeEntity source)
  {
    AttributeModel destination = new()
    {
      Id = source.Id,
      Slug = source.Slug,
      Name = source.Name,
      Category = source.Category,
      Value = source.Value,
      MetaDescription = source.MetaDescription,
      Summary = source.Summary,
      HtmlContent = source.HtmlContent
    };

    foreach (StatisticEntity statistic in source.Statistics)
    {
      if (statistic.IsPublished)
      {
        destination.Statistics.Add(ToStatistic(statistic, destination));
      }
    }
    foreach (SkillEntity skill in source.Skills)
    {
      if (skill.IsPublished)
      {
        destination.Skills.Add(ToSkill(skill, destination));
      }
    }

    MapAggregate(source, destination);

    return destination;
  }

  public CasteModel ToCaste(CasteEntity source)
  {
    CasteModel destination = new()
    {
      Id = source.Id,
      Slug = source.Slug,
      Name = source.Name,
      WealthRoll = source.WealthRoll,
      MetaDescription = source.MetaDescription,
      Summary = source.Summary,
      HtmlContent = source.HtmlContent
    };

    if (source.Skill is not null)
    {
      if (source.Skill.IsPublished)
      {
        destination.Skill = ToSkill(source.Skill);
      }
    }
    else if (source.SkillId.HasValue)
    {
      throw new ArgumentException("The skill is required.", nameof(source));
    }

    if (source.Feature is not null)
    {
      if (source.Feature.IsPublished)
      {
        destination.Feature = ToFeature(source.Feature);
      }
    }
    else if (source.FeatureId.HasValue)
    {
      throw new ArgumentException("The feature is required.", nameof(source));
    }

    MapAggregate(source, destination);

    return destination;
  }

  public CustomizationModel ToCustomization(CustomizationEntity source)
  {
    CustomizationModel destination = new()
    {
      Id = source.Id,
      Slug = source.Slug,
      Name = source.Name,
      Kind = source.Kind,
      MetaDescription = source.MetaDescription,
      Summary = source.Summary,
      HtmlContent = source.HtmlContent
    };

    MapAggregate(source, destination);

    return destination;
  }

  public EducationModel ToEducation(EducationEntity source)
  {
    EducationModel destination = new()
    {
      Id = source.Id,
      Slug = source.Slug,
      Name = source.Name,
      WealthMultiplier = source.WealthMultiplier,
      MetaDescription = source.MetaDescription,
      Summary = source.Summary,
      HtmlContent = source.HtmlContent
    };

    if (source.Skill is not null)
    {
      if (source.Skill.IsPublished)
      {
        destination.Skill = ToSkill(source.Skill);
      }
    }
    else if (source.SkillId.HasValue)
    {
      throw new ArgumentException("The skill is required.", nameof(source));
    }

    if (source.Feature is not null)
    {
      if (source.Feature.IsPublished)
      {
        destination.Feature = ToFeature(source.Feature);
      }
    }
    else if (source.FeatureId.HasValue)
    {
      throw new ArgumentException("The feature is required.", nameof(source));
    }

    MapAggregate(source, destination);

    return destination;
  }

  public EthnicityModel ToEthnicity(LineageEntity source)
  {
    EthnicityModel destination = new();

    MapLineage(source, destination);

    if (source.Parent is not null && source.Parent.IsPublished)
    {
      destination.Species = ToSpecies(source.Parent);
    }

    return destination;
  }

  public FeatureModel ToFeature(FeatureEntity source)
  {
    FeatureModel destination = new()
    {
      Id = source.Id,
      Key = source.Key,
      Name = source.Name,
      HtmlContent = source.HtmlContent
    };

    MapAggregate(source, destination);

    return destination;
  }

  public LanguageModel ToLanguage(LanguageEntity source)
  {
    LanguageModel destination = new()
    {
      Id = source.Id,
      Slug = source.Slug,
      Name = source.Name,
      TypicalSpeakers = source.TypicalSpeakers,
      MetaDescription = source.MetaDescription,
      Summary = source.Summary,
      HtmlContent = source.HtmlContent
    };

    if (source.Script is not null)
    {
      if (source.Script.IsPublished)
      {
        destination.Script = ToScript(source.Script);
      }
    }
    else if (source.ScriptId.HasValue)
    {
      throw new ArgumentException("The script is required.", nameof(source));
    }

    MapAggregate(source, destination);

    return destination;
  }

  public LineageModel ToLineage(LineageEntity source)
  {
    LineageModel destination = new();

    MapLineage(source, destination);

    if (source.Parent is not null && source.Parent.IsPublished)
    {
      destination.Parent = ToLineage(source.Parent);
    }

    return destination;
  }

  public ScriptModel ToScript(ScriptEntity source)
  {
    ScriptModel destination = new()
    {
      Id = source.Id,
      Slug = source.Slug,
      Name = source.Name,
      MetaDescription = source.MetaDescription,
      Summary = source.Summary,
      HtmlContent = source.HtmlContent
    };

    MapAggregate(source, destination);

    return destination;
  }

  public SkillModel ToSkill(SkillEntity source) => ToSkill(source, attribute: null);
  public SkillModel ToSkill(SkillEntity source, AttributeModel? attribute)
  {
    SkillModel destination = new()
    {
      Id = source.Id,
      Slug = source.Slug,
      Name = source.Name,
      Value = source.Value,
      MetaDescription = source.MetaDescription,
      Summary = source.Summary,
      HtmlContent = source.HtmlContent
    };

    if (attribute is not null)
    {
      destination.Attribute = attribute;
    }
    else if (source.Attribute is null)
    {
      if (source.AttributeId.HasValue)
      {
        throw new ArgumentException("The attribute is required.", nameof(source));
      }
    }
    else if (source.Attribute.IsPublished)
    {
      destination.Attribute = ToAttribute(source.Attribute);
    }

    MapAggregate(source, destination);

    return destination;
  }

  public SpecializationModel ToSpecialization(SpecializationEntity source)
  {
    SpecializationModel destination = new()
    {
      Id = source.Id,
      Slug = source.Slug,
      Name = source.Name,
      Tier = source.Tier,
      MetaDescription = source.MetaDescription,
      Summary = source.Summary,
      HtmlContent = source.HtmlContent
    };

    if (source.MandatoryTalent is not null)
    {
      if (source.MandatoryTalent.IsPublished)
      {
        destination.Requirements.Talent = ToTalent(source.MandatoryTalent);
      }
    }
    else if (source.MandatoryTalentId.HasValue)
    {
      throw new ArgumentException("The mandatory talent is required.", nameof(source));
    }
    if (source.OtherRequirements is not null)
    {
      destination.Requirements.Other.AddRange(SplitOnNewLine(source.OtherRequirements));
    }

    foreach (SpecializationOptionalTalentEntity optionalTalent in source.OptionalTalents)
    {
      TalentEntity talent = optionalTalent.Talent
        ?? throw new ArgumentException($"The optional talent is required (SpecializationId={optionalTalent.SpecializationId}, TalentId={optionalTalent.TalentId}).", nameof(source));
      if (talent.IsPublished)
      {
        destination.Options.Talents.Add(ToTalent(talent));
      }
    }
    if (source.OtherOptions is not null)
    {
      destination.Options.Other.AddRange(SplitOnNewLine(source.OtherOptions));
    }

    DoctrineEntity? doctrine = source.Doctrine;
    if (doctrine is not null)
    {
      destination.Doctrine = new DoctrineModel(doctrine.Name);
      if (doctrine.HtmlContent is not null)
      {
        destination.Doctrine.Description.AddRange(SplitOnNewLine(doctrine.HtmlContent));
      }
      foreach (DoctrineDiscountedTalentEntity discountedTalent in doctrine.DiscountedTalents)
      {
        TalentEntity talent = discountedTalent.Talent
          ?? throw new ArgumentException($"The discounted talent is required (DoctrineId={discountedTalent.DoctrineId}, TalentId={discountedTalent.TalentId}).", nameof(source));
        if (talent.IsPublished)
        {
          destination.Doctrine.DiscountedTalents.Add(ToTalent(talent));
        }
      }
      foreach (DoctrineFeatureEntity doctrineFeature in doctrine.Features)
      {
        FeatureEntity feature = doctrineFeature.Feature
          ?? throw new ArgumentException($"The feature is required (DoctrineId={doctrineFeature.DoctrineId}, FeatureId={doctrineFeature.FeatureId}).", nameof(source));
        if (feature.IsPublished)
        {
          destination.Doctrine.Features.Add(ToFeature(feature));
        }
      }
    }

    MapAggregate(source, destination);

    return destination;
  }

  public SpeciesModel ToSpecies(LineageEntity source)
  {
    SpeciesModel destination = new();

    MapLineage(source, destination);

    return destination;
  }

  public SpellModel ToSpell(SpellEntity source)
  {
    SpellModel destination = new()
    {
      Id = source.Id,
      Slug = source.Slug,
      Name = source.Name,
      Tier = source.Tier,
      MetaDescription = source.MetaDescription,
      Summary = source.Summary,
      HtmlContent = source.HtmlContent
    };

    foreach (SpellEffectEntity effect in source.Effects)
    {
      if (effect.IsPublished)
      {
        destination.Abilities.Add(ToSpellAbility(effect));
      }
    }

    MapAggregate(source, destination);

    return destination;
  }

  public static SpellAbilityModel ToSpellAbility(SpellEffectEntity source)
  {
    SpellAbilityModel destination = new()
    {
      Level = source.Level,
      Name = source.Name,
      Range = source.Range,
      HtmlContent = source.HtmlContent
    };

    destination.Casting.Time = source.CastingTime;
    destination.Casting.Ritual = source.IsRitual;

    if (source.Duration.HasValue && source.DurationUnit.HasValue)
    {
      destination.Duration = new SpellDurationModel
      {
        Value = source.Duration.Value,
        Unit = source.DurationUnit.Value,
        Concentration = source.IsConcentration
      };
    }

    destination.Components.Focus = source.Focus;
    destination.Components.Material = source.Material;
    destination.Components.Somatic = source.IsSomatic;
    destination.Components.Verbal = source.IsVerbal;

    return destination;
  }

  public StatisticModel ToStatistic(StatisticEntity source) => ToStatistic(source, attribute: null);
  public StatisticModel ToStatistic(StatisticEntity source, AttributeModel? attribute)
  {
    StatisticModel destination = new()
    {
      Id = source.Id,
      Slug = source.Slug,
      Name = source.Name,
      Value = source.Value,
      MetaDescription = source.MetaDescription,
      Summary = source.Summary,
      HtmlContent = source.HtmlContent
    };

    if (attribute is not null)
    {
      destination.Attribute = attribute;
    }
    else if (source.Attribute is null)
    {
      throw new ArgumentException("The attribute is required.", nameof(source));
    }
    else if (source.Attribute.IsPublished)
    {
      destination.Attribute = ToAttribute(source.Attribute);
    }

    MapAggregate(source, destination);

    return destination;
  }

  public TalentModel ToTalent(TalentEntity source)
  {
    TalentModel destination = new()
    {
      Id = source.Id,
      Slug = source.Slug,
      Name = source.Name,
      Tier = source.Tier,
      AllowMultiplePurchases = source.AllowMultiplePurchases,
      MetaDescription = source.MetaDescription,
      Summary = source.Summary,
      HtmlContent = source.HtmlContent
    };

    if (source.Skill is not null)
    {
      if (source.Skill.IsPublished)
      {
        destination.Skill = ToSkill(source.Skill);
      }
    }
    else if (source.SkillId.HasValue)
    {
      throw new ArgumentException("The skill is required.", nameof(source));
    }

    if (source.RequiredTalent is not null && source.RequiredTalent.IsPublished)
    {
      destination.RequiredTalent = ToTalent(source.RequiredTalent);
    }

    MapAggregate(source, destination);

    return destination;
  }

  private void MapLineage(LineageEntity source, LineageBase destination)
  {
    destination.Id = source.Id;
    destination.Slug = source.Slug;
    destination.Name = source.Name;
    destination.MetaDescription = source.MetaDescription;
    destination.Summary = source.Summary;
    destination.HtmlContent = source.HtmlContent;

    if (source.FamilyNames is not null)
    {
      destination.Names.Family.AddRange(source.FamilyNames.Split(Constants.Separator));
    }
    if (source.FemaleNames is not null)
    {
      destination.Names.Female.AddRange(source.FemaleNames.Split(Constants.Separator));
    }
    if (source.MaleNames is not null)
    {
      destination.Names.Male.AddRange(source.MaleNames.Split(Constants.Separator));
    }
    if (source.UnisexNames is not null)
    {
      destination.Names.Unisex.AddRange(source.UnisexNames.Split(Constants.Separator));
    }
    if (source.CustomNames is not null)
    {
      Dictionary<string, string[]>? customNames = JsonSerializer.Deserialize<Dictionary<string, string[]>>(source.CustomNames);
      if (customNames is not null)
      {
        foreach (KeyValuePair<string, string[]> category in customNames)
        {
          destination.Names.Custom.Add(new NameCategory(category.Key, category.Value));
        }
      }
    }
    destination.Names.Text = source.NamesText;

    destination.Speeds.Walk = source.Walk;
    destination.Speeds.Climb = source.Climb;
    destination.Speeds.Swim = source.Swim;
    destination.Speeds.Fly = source.Fly;
    destination.Speeds.Hover = source.Hover;
    destination.Speeds.Burrow = source.Burrow;

    destination.Size.Category = source.SizeCategory;
    destination.Size.Roll = source.SizeRoll;

    destination.Weight.Malnutrition = source.Malnutrition;
    destination.Weight.Skinny = source.Skinny;
    destination.Weight.Normal = source.NormalWeight;
    destination.Weight.Overweight = source.Overweight;
    destination.Weight.Obese = source.Obese;

    destination.Age.Teenager = source.Teenager;
    destination.Age.Adult = source.Adult;
    destination.Age.Mature = source.Mature;
    destination.Age.Venerable = source.Venerable;

    foreach (LineageFeatureEntity lineageFeature in source.Features)
    {
      FeatureEntity feature = lineageFeature.Feature
        ?? throw new ArgumentException($"The feature is required (LineageId={lineageFeature.LineageId}, FeatureId={lineageFeature.FeatureId}).", nameof(source));
      if (feature.IsPublished)
      {
        destination.Features.Add(ToFeature(feature));
      }
    }

    foreach (LineageLanguageEntity lineageLanguage in source.Languages)
    {
      LanguageEntity language = lineageLanguage.Language
        ?? throw new ArgumentException($"The language is required (LineageId={lineageLanguage.LineageId}, LanguageId={lineageLanguage.LanguageId}).", nameof(source));
      if (language.IsPublished)
      {
        destination.Languages.Items.Add(ToLanguage(language));
      }
    }
    destination.Languages.Extra = source.ExtraLanguages;
    destination.Languages.Text = source.LanguagesText;

    MapAggregate(source, destination);
  }

  private void MapAggregate(AggregateEntity source, Aggregate destination)
  {
    destination.Version = source.Version;

    destination.CreatedBy = FindActor(source.CreatedBy);
    destination.CreatedOn = source.CreatedOn.AsUniversalTime();

    destination.UpdatedBy = FindActor(source.UpdatedBy);
    destination.UpdatedOn = source.UpdatedOn.AsUniversalTime();
  }

  private Actor FindActor(string? id) => TryFindActor(id) ?? _system;
  private Actor FindActor(ActorId? id) => TryFindActor(id) ?? _system;

  private Actor? TryFindActor(string? id) => TryFindActor(string.IsNullOrWhiteSpace(id) ? null : new ActorId(id));
  private Actor? TryFindActor(ActorId? id) => id.HasValue && _actors.TryGetValue(id.Value, out Actor? actor) ? actor : null;

  private static IReadOnlyCollection<string> SplitOnNewLine(string text) => text.Remove("\r").Split('\n')
    .Where(x => !string.IsNullOrWhiteSpace(x))
    .Select(x => x.Trim())
    .ToList().AsReadOnly();
}
