using Krakenar.Contracts.Search;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Cms.Core.Lineages;
using SkillCraft.Cms.Core.Lineages.Models;
using SkillCraft.Cms.Models.Parameters;

namespace SkillCraft.Cms.Controllers;

[ApiController]
[Route("api/ethnicities")]
public class EthnicityController : ControllerBase
{
  private readonly ILineageService _lineageService;

  public EthnicityController(ILineageService lineageService)
  {
    _lineageService = lineageService;
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<EthnicityModel>> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    EthnicityModel? ethnicity = await _lineageService.ReadEthnicityAsync(id, slug: null, cancellationToken);
    return ethnicity is null ? NotFound() : Ok(ethnicity);
  }

  [HttpGet("slug:{slug}")] // TODO(fpion): hierarchical URL
  public async Task<ActionResult<EthnicityModel>> ReadAsync(string slug, CancellationToken cancellationToken)
  {
    EthnicityModel? ethnicity = await _lineageService.ReadEthnicityAsync(id: null, slug, cancellationToken);
    return ethnicity is null ? NotFound() : Ok(ethnicity);
  }

  [HttpGet("/api/species/{idOrSlug}/ethnicities")]
  public async Task<ActionResult<SearchResults<EthnicityModel>>> SearchAsync(string idOrSlug, [FromQuery] SearchEthnicitiesParameters parameters, CancellationToken cancellationToken)
  {
    SearchEthnicitiesPayload payload = parameters.ToPayload();
    payload.Species = idOrSlug;

    SearchResults<EthnicityModel> ethnicities = await _lineageService.SearchAsync(payload, cancellationToken);
    return Ok(ethnicities);
  }
}
