using Krakenar.Contracts.Search;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Cms.Core.Lineages;
using SkillCraft.Cms.Core.Lineages.Models;
using SkillCraft.Cms.Models.Parameters;

namespace SkillCraft.Cms.Controllers;

[ApiController]
[Route("api/lineages")]
public class LineageController : ControllerBase
{
  private readonly ILineageService _lineageService;

  public LineageController(ILineageService lineageService)
  {
    _lineageService = lineageService;
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<LineageModel>> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    LineageModel? lineage = await _lineageService.ReadAsync(id, cancellationToken);
    return lineage is null ? NotFound() : Ok(lineage);
  }

  [HttpGet]
  public async Task<ActionResult<SearchResults<LineageModel>>> SearchAsync([FromQuery] SearchLineagesParameters parameters, CancellationToken cancellationToken)
  {
    SearchLineagesPayload payload = parameters.ToPayload();
    SearchResults<LineageModel> lineages = await _lineageService.SearchAsync(payload, cancellationToken);
    return Ok(lineages);
  }
}
