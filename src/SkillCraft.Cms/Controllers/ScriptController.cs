using Krakenar.Contracts.Search;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Cms.Core.Scripts;
using SkillCraft.Cms.Core.Scripts.Models;
using SkillCraft.Cms.Models.Parameters;

namespace SkillCraft.Cms.Controllers;

[ApiController]
[Route("api/scripts")]
public class ScriptController : ControllerBase
{
  private readonly IScriptService _scriptService;

  public ScriptController(IScriptService scriptService)
  {
    _scriptService = scriptService;
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<ScriptModel>> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    ScriptModel? script = await _scriptService.ReadAsync(id, slug: null, cancellationToken);
    return script is null ? NotFound() : Ok(script);
  }

  [HttpGet("slug:{slug}")]
  public async Task<ActionResult<ScriptModel>> ReadAsync(string slug, CancellationToken cancellationToken)
  {
    ScriptModel? script = await _scriptService.ReadAsync(id: null, slug, cancellationToken);
    return script is null ? NotFound() : Ok(script);
  }

  [HttpGet]
  public async Task<ActionResult<SearchResults<ScriptModel>>> SearchAsync([FromQuery] SearchScriptsParameters parameters, CancellationToken cancellationToken)
  {
    SearchScriptsPayload payload = parameters.ToPayload();
    SearchResults<ScriptModel> scripts = await _scriptService.SearchAsync(payload, cancellationToken);
    return Ok(scripts);
  }
}
