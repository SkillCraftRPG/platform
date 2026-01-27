using Krakenar.Core.Configurations.Commands;
using Logitar.CQRS;
using SkillCraft.Cms.Seeding.Settings;

namespace SkillCraft.Cms.Seeding.Krakenar.Tasks;

internal class InitializeConfigurationTask : SeedingTask
{
  public override string? Description => "Migrates Krakenar database.";
}

internal class InitializeConfigurationTaskHandler : ICommandHandler<InitializeConfigurationTask, Unit>
{
  private readonly DefaultSettings _defaults;
  private readonly ICommandHandler<InitializeConfiguration, Unit> _handler;

  public InitializeConfigurationTaskHandler(DefaultSettings defaults, ICommandHandler<InitializeConfiguration, Unit> handler)
  {
    _defaults = defaults;
    _handler = handler;
  }

  public async Task<Unit> HandleAsync(InitializeConfigurationTask task, CancellationToken cancellationToken)
  {
    InitializeConfiguration command = new(_defaults.Locale, _defaults.UniqueName, _defaults.Password);
    await _handler.HandleAsync(command, cancellationToken);

    return Unit.Value;
  }
}
