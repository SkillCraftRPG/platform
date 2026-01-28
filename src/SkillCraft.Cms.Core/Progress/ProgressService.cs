using Logitar.CQRS;
using Microsoft.Extensions.DependencyInjection;
using SkillCraft.Cms.Core.Progress.Models;
using SkillCraft.Cms.Core.Progress.Queries;

namespace SkillCraft.Cms.Core.Progress;

public interface IProgressService
{
  Task<ProgressModel> ReadAsync(CancellationToken cancellationToken = default);
}

internal class ProgressService : IProgressService
{
  public static void Register(IServiceCollection services)
  {
    services.AddTransient<IProgressService, ProgressService>();
    services.AddTransient<IQueryHandler<ReadProgressQuery, ProgressModel>, ReadProgressQueryHandler>();
  }

  private readonly IQueryBus _queryBus;

  public ProgressService(IQueryBus queryBus)
  {
    _queryBus = queryBus;
  }

  public async Task<ProgressModel> ReadAsync(CancellationToken cancellationToken)
  {
    ReadProgressQuery query = new();
    return await _queryBus.ExecuteAsync(query, cancellationToken);
  }
}
