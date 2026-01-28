using Logitar.CQRS;
using SkillCraft.Cms.Core.Progress;
using SkillCraft.Cms.Export.Tasks;
using SkillCraft.Cms.Infrastructure.Contents;

namespace SkillCraft.Cms.Export;

internal class ExportWorker : BackgroundService
{
  private const string GenericErrorMessage = "An unhanded exception occurred.";

  private readonly IHostApplicationLifetime _hostApplicationLifetime;
  private readonly ILogger<ExportWorker>? _logger;
  private readonly IServiceProvider _serviceProvider;

  private ICommandBus? _commandBus = null;
  private ICommandBus CommandBus => _commandBus ?? throw new InvalidOperationException("The command bus has not been initialized.");

  private LogLevel _result = LogLevel.Information; // NOTE(fpion): "Information" means success.

  public ExportWorker(IServiceProvider serviceProvider)
  {
    _hostApplicationLifetime = serviceProvider.GetRequiredService<IHostApplicationLifetime>();
    _logger = serviceProvider.GetService<ILogger<ExportWorker>>();
    _serviceProvider = serviceProvider;
  }

  protected override async Task ExecuteAsync(CancellationToken cancellationToken)
  {
    Stopwatch chrono = Stopwatch.StartNew();
    _logger?.LogInformation("Worker executing at {Timestamp}.", DateTimeOffset.Now);

    using IServiceScope scope = _serviceProvider.CreateScope();
    _commandBus = scope.ServiceProvider.GetService<ICommandBus>();

    try
    {
      // NOTE(fpion): the order of these tasks matter.
      await ExecuteAsync(new ExportContentsTask(AttributeDefinition.ContentTypeId, "output/data/attributes"), cancellationToken);
      await ExecuteAsync(new ExportContentsTask(StatisticDefinition.ContentTypeId, "output/data/statistics"), cancellationToken);
      await ExecuteAsync(new ExportContentsTask(SkillDefinition.ContentTypeId, "output/data/skills"), cancellationToken);
      await ExecuteAsync(new ExportContentsTask(GiftDefinition.ContentTypeId, "output/data/gifts"), cancellationToken);
      await ExecuteAsync(new ExportContentsTask(DisabilityDefinition.ContentTypeId, "output/data/disabilities"), cancellationToken);
      await ExecuteAsync(new ExportContentsTask(FeatureDefinition.ContentTypeId, "output/data/features"), cancellationToken);
      await ExecuteAsync(new ExportContentsTask(CasteDefinition.ContentTypeId, "output/data/castes"), cancellationToken);
      await ExecuteAsync(new ExportContentsTask(EducationDefinition.ContentTypeId, "output/data/educations"), cancellationToken);
      await ExecuteAsync(new ExportContentsTask(TalentDefinition.ContentTypeId, "output/data/talents"), cancellationToken);
      await ExecuteAsync(new ExportContentsTask(ScriptDefinition.ContentTypeId, "output/data/scripts"), cancellationToken);
      await ExecuteAsync(new ExportContentsTask(LanguageDefinition.ContentTypeId, "output/data/languages"), cancellationToken);
      await ExecuteAsync(new ExportContentsTask(LineageDefinition.ContentTypeId, "output/data/lineages"), cancellationToken);
      await ExecuteAsync(new ExportContentsTask(SpecializationDefinition.ContentTypeId, "output/data/specializations"), cancellationToken);
      await ExecuteAsync(new ExportContentsTask(DoctrineDefinition.ContentTypeId, "output/data/doctrines"), cancellationToken);
      await ExecuteAsync(new ExportContentsTask(SpellDefinition.ContentTypeId, "output/data/spells"), cancellationToken);
      await ExecuteAsync(new ExportContentsTask(SpellEffectDefinition.ContentTypeId, "output/data/spell_effects"), cancellationToken);
      await ExecuteAsync(new ExportContentsTask(ProgressDefinition.ContentTypeId, "output/data/progress"), cancellationToken);
      await ExecuteAsync(new ExportContentsTask(CollectionDefinition.ContentTypeId, "output/data/collections"), cancellationToken);
      await ExecuteAsync(new ExportContentsTask(ArticleDefinition.ContentTypeId, "output/data/articles"), cancellationToken);
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
            _logger.LogError("Export failed after {Elapsed}ms ({Seconds} {SecondText}).", chrono.ElapsedMilliseconds, seconds, secondText);
            break;
          case LogLevel.Warning:
            _logger.LogWarning("Export completed with warnings in {Elapsed}ms ({Seconds} {SecondText}).", chrono.ElapsedMilliseconds, seconds, secondText);
            break;
          default:
            _logger.LogInformation("Export succeeded in {Elapsed}ms ({Seconds} {SecondText}).", chrono.ElapsedMilliseconds, seconds, secondText);
            break;
        }
      }

      _hostApplicationLifetime.StopApplication();
    }
  }

  private async Task ExecuteAsync(ExportTask task, CancellationToken cancellationToken)
  {
    await ExecuteAsync(task, continueOnError: false, cancellationToken);
  }
  private async Task ExecuteAsync(ExportTask task, bool continueOnError, CancellationToken cancellationToken)
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
