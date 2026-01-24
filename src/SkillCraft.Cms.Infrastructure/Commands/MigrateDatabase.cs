using Krakenar.EntityFrameworkCore.Relational;
using Krakenar.Infrastructure.Commands;
using Logitar.CQRS;
using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Microsoft.EntityFrameworkCore;

namespace SkillCraft.Cms.Infrastructure.Commands;

internal class MigrateDatabaseCommandHandler : Krakenar.EntityFrameworkCore.Relational.Handlers.MigrateDatabaseCommandHandler, ICommandHandler<MigrateDatabase, Unit>
{
  private readonly RulesContext _rulesContext;

  public MigrateDatabaseCommandHandler(EventContext eventContext, KrakenarContext krakenarContext, RulesContext rulesContext)
    : base(eventContext, krakenarContext)
  {
    _rulesContext = rulesContext;
  }

  public override async Task<Unit> HandleAsync(MigrateDatabase command, CancellationToken cancellationToken)
  {
    await base.HandleAsync(command, cancellationToken);

    await _rulesContext.Database.MigrateAsync(cancellationToken);

    return Unit.Value;
  }
}
