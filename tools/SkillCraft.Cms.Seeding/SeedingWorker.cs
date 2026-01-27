using Krakenar.Contracts.Users;
using Krakenar.Core;
using Krakenar.Core.Users;
using Logitar.CQRS;
using Logitar.EventSourcing;
using SkillCraft.Cms.Infrastructure.Contents;
using SkillCraft.Cms.Seeding.Krakenar.Tasks;
using SkillCraft.Cms.Seeding.Settings;
using User = Krakenar.Contracts.Users.User;

namespace SkillCraft.Cms.Seeding;

internal class SeedingWorker : BackgroundService
{
  private const string GenericErrorMessage = "An unhanded exception occurred.";

  private readonly SeedingApplicationContext _applicationContext;
  private readonly IHostApplicationLifetime _hostApplicationLifetime;
  private readonly ILogger<SeedingWorker>? _logger;
  private readonly IServiceProvider _serviceProvider;

  private ICommandBus? _commandBus = null;
  private ICommandBus CommandBus => _commandBus ?? throw new InvalidOperationException("The command bus has not been initialized.");

  private LogLevel _result = LogLevel.Information; // NOTE(fpion): "Information" means success.

  public SeedingWorker(IServiceProvider serviceProvider)
  {
    _applicationContext = (SeedingApplicationContext)serviceProvider.GetRequiredService<IApplicationContext>();
    _hostApplicationLifetime = serviceProvider.GetRequiredService<IHostApplicationLifetime>();
    _logger = serviceProvider.GetService<ILogger<SeedingWorker>>();
    _serviceProvider = serviceProvider;
  }

  protected override async Task ExecuteAsync(CancellationToken cancellationToken)
  {
    Stopwatch chrono = Stopwatch.StartNew();
    _logger?.LogInformation("Worker executing at {Timestamp}.", DateTimeOffset.Now);

    using IServiceScope scope = _serviceProvider.CreateScope();
    _commandBus = scope.ServiceProvider.GetService<ICommandBus>();

    DefaultSettings defaults = _serviceProvider.GetRequiredService<DefaultSettings>();

    try
    {
      // NOTE(fpion): the order of these tasks matter.
      await ExecuteAsync(new MigrateDatabaseTask(), cancellationToken);
      await ExecuteAsync(new InitializeConfigurationTask(), cancellationToken);

      IUserService userService = scope.ServiceProvider.GetRequiredService<IUserService>();
      User user = await userService.ReadAsync(id: null, defaults.UniqueName, customIdentifier: null, cancellationToken)
        ?? throw new InvalidOperationException($"The user 'UniqueName={defaults.UniqueName}' was not found.");
      _applicationContext.ActorId = new ActorId(new UserId(user.Id).Value);

      await ExecuteAsync(new SeedUsersTask(), cancellationToken);
      await ExecuteAsync(new SeedContentTypesTask(), cancellationToken);
      await ExecuteAsync(new SeedFieldTypesTask(), cancellationToken);
      await ExecuteAsync(new SeedContentTypesTask(fieldDefinitions: true), cancellationToken);

      await ExecuteAsync(new SeedContentsTask(AttributeDefinition.ContentTypeId, defaults.Locale, "Krakenar/data/attributes"), cancellationToken);
      await ExecuteAsync(new SeedContentsTask(StatisticDefinition.ContentTypeId, defaults.Locale, "Krakenar/data/statistics"), cancellationToken);
      await ExecuteAsync(new SeedContentsTask(SkillDefinition.ContentTypeId, defaults.Locale, "Krakenar/data/skills"), cancellationToken);
      await ExecuteAsync(new SeedContentsTask(GiftDefinition.ContentTypeId, defaults.Locale, "Krakenar/data/gifts"), cancellationToken);
      await ExecuteAsync(new SeedContentsTask(DisabilityDefinition.ContentTypeId, defaults.Locale, "Krakenar/data/disabilities"), cancellationToken);
      await ExecuteAsync(new SeedContentsTask(FeatureDefinition.ContentTypeId, defaults.Locale, "Krakenar/data/features"), cancellationToken);
      await ExecuteAsync(new SeedContentsTask(CasteDefinition.ContentTypeId, defaults.Locale, "Krakenar/data/castes"), cancellationToken);
      await ExecuteAsync(new SeedContentsTask(EducationDefinition.ContentTypeId, defaults.Locale, "Krakenar/data/educations"), cancellationToken);
      await ExecuteAsync(new SeedContentsTask(TalentDefinition.ContentTypeId, defaults.Locale, "Krakenar/data/talents"), cancellationToken);
      await ExecuteAsync(new SeedContentsTask(ScriptDefinition.ContentTypeId, defaults.Locale, "Krakenar/data/scripts"), cancellationToken);
      await ExecuteAsync(new SeedContentsTask(LanguageDefinition.ContentTypeId, defaults.Locale, "Krakenar/data/languages"), cancellationToken);
      await ExecuteAsync(new SeedContentsTask(LineageDefinition.ContentTypeId, defaults.Locale, "Krakenar/data/lineages"), cancellationToken);
      await ExecuteAsync(new SeedContentsTask(SpecializationDefinition.ContentTypeId, defaults.Locale, "Krakenar/data/specializations"), cancellationToken);
      await ExecuteAsync(new SeedContentsTask(DoctrineDefinition.ContentTypeId, defaults.Locale, "Krakenar/data/doctrines"), cancellationToken);
      await ExecuteAsync(new SeedContentsTask(SpellDefinition.ContentTypeId, defaults.Locale, "Krakenar/data/spells"), cancellationToken);
      await ExecuteAsync(new SeedContentsTask(SpellEffectDefinition.ContentTypeId, defaults.Locale, "Krakenar/data/spell_effects"), cancellationToken);
      await ExecuteAsync(new SeedContentsTask(ProgressDefinition.ContentTypeId, defaults.Locale, "Krakenar/data/progress"), cancellationToken);
    }
    catch (Exception exception)
    {
      _logger?.LogError(exception, GenericErrorMessage);
      _result = LogLevel.Error;

      Environment.ExitCode = exception.HResult;
    }
    finally
    {
      chrono.Stop();

      if (_logger is not null)
      {
        long seconds = chrono.ElapsedMilliseconds / 1000;
        string secondText = seconds <= 1 ? "second" : "seconds";
        switch (_result)
        {
          case LogLevel.Error:
            _logger.LogError("Seeding failed after {Elapsed}ms ({Seconds} {SecondText}).", chrono.ElapsedMilliseconds, seconds, secondText);
            break;
          case LogLevel.Warning:
            _logger.LogWarning("Seeding completed with warnings in {Elapsed}ms ({Seconds} {SecondText}).", chrono.ElapsedMilliseconds, seconds, secondText);
            break;
          default:
            _logger.LogInformation("Seeding succeeded in {Elapsed}ms ({Seconds} {SecondText}).", chrono.ElapsedMilliseconds, seconds, secondText);
            break;
        }
      }

      _hostApplicationLifetime.StopApplication();
    }
  }

  private async Task ExecuteAsync(SeedingTask task, CancellationToken cancellationToken)
  {
    await ExecuteAsync(task, continueOnError: false, cancellationToken);
  }
  private async Task ExecuteAsync(SeedingTask task, bool continueOnError, CancellationToken cancellationToken)
  {
    bool hasFailed = false;
    try
    {
      await CommandBus.ExecuteAsync(task, cancellationToken);
    }
    catch (Exception exception)
    {
      if (continueOnError)
      {
        _logger?.LogWarning(exception, GenericErrorMessage);
        hasFailed = true;
      }
      else
      {
        throw;
      }
    }
    finally
    {
      task.Complete();

      LogLevel result = LogLevel.Information;
      if (hasFailed)
      {
        _result = LogLevel.Warning;
        result = LogLevel.Warning;
      }

      if (_logger is not null)
      {
        int milliseconds = task.Duration?.Milliseconds ?? 0;
        int seconds = milliseconds / 1000;
        string secondText = seconds <= 1 ? "second" : "seconds";
        _logger.Log(result, "Task '{Name}' succeeded in {Elapsed}ms ({Seconds} {SecondText}).", task.Name, milliseconds, seconds, secondText);
      }
    }
  }
}
