using Krakenar.Contracts.Search;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Cms.Core.Lineages;
using SkillCraft.Cms.Core.Lineages.Models;
using SkillCraft.Cms.Models.Parameters;

namespace SkillCraft.Cms.Controllers;

[ApiController]
[Route("api/species")]
public class SpeciesController : ControllerBase
{
  private readonly ILineageService _lineageService;

  public SpeciesController(ILineageService lineageService)
  {
    _lineageService = lineageService;
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<SpeciesModel>> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    SpeciesModel? species = await _lineageService.ReadSpeciesAsync(id, slug: null, cancellationToken);
    return species is null ? NotFound() : Ok(species);
  }

  [HttpGet("slug:{slug}")]
  public async Task<ActionResult<SpeciesModel>> ReadAsync(string slug, CancellationToken cancellationToken)
  {
    SpeciesModel? species = await _lineageService.ReadSpeciesAsync(id: null, slug, cancellationToken);
    return species is null ? NotFound() : Ok(species);
  }

  [HttpGet]
  public async Task<ActionResult<SearchResults<SpeciesModel>>> SearchAsync([FromQuery] SearchSpeciesParameters parameters, CancellationToken cancellationToken)
  {
    SearchSpeciesPayload payload = parameters.ToPayload();
    SearchResults<SpeciesModel> lineages = await _lineageService.SearchAsync(payload, cancellationToken);
    return Ok(lineages);
  }
}
