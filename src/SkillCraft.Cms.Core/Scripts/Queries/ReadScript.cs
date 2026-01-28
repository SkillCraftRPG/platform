using Krakenar.Contracts;
using Logitar.CQRS;
using SkillCraft.Cms.Core.Scripts.Models;

namespace SkillCraft.Cms.Core.Scripts.Queries;

internal record ReadScriptQuery(Guid? Id, string? Slug) : IQuery<ScriptModel?>;

internal class ReadScriptQueryHandler : IQueryHandler<ReadScriptQuery, ScriptModel?>
{
  private readonly IScriptQuerier _scriptQuerier;

  public ReadScriptQueryHandler(IScriptQuerier scriptQuerier)
  {
    _scriptQuerier = scriptQuerier;
  }

  public async Task<ScriptModel?> HandleAsync(ReadScriptQuery query, CancellationToken cancellationToken)
  {
    Dictionary<Guid, ScriptModel> scripts = new(capacity: 2);

    if (query.Id.HasValue)
    {
      ScriptModel? script = await _scriptQuerier.ReadAsync(query.Id.Value, cancellationToken);
      if (script is not null)
      {
        scripts[script.Id] = script;
      }
    }

    if (!string.IsNullOrWhiteSpace(query.Slug))
    {
      ScriptModel? script = await _scriptQuerier.ReadAsync(query.Slug, cancellationToken);
      if (script is not null)
      {
        scripts[script.Id] = script;
      }
    }

    if (scripts.Count > 1)
    {
      throw TooManyResultsException<ScriptModel>.ExpectedSingle(scripts.Count);
    }

    return scripts.Values.SingleOrDefault();
  }
}
