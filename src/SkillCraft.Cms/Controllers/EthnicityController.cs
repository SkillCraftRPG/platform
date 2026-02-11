using Krakenar.Contracts.Search;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Cms.Core.Lineages;
using SkillCraft.Cms.Core.Lineages.Models;
using SkillCraft.Cms.Models.Parameters;

namespace SkillCraft.Cms.Controllers;

[ApiController]
[Route("/api/species/{species}/ethnicities")]
public class EthnicityController : ControllerBase
{
  private readonly ILineageService _lineageService;

  public EthnicityController(ILineageService lineageService)
  {
    _lineageService = lineageService;
  }

  [HttpGet("/api/ethnicities/{id}")]
  public async Task<ActionResult<EthnicityModel>> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    EthnicityModel? ethnicity = await _lineageService.ReadEthnicityAsync(id, path: null, cancellationToken);
    return ethnicity is null ? NotFound() : Ok(ethnicity);
  }

  [HttpGet("{ethnicity}")]
  public async Task<ActionResult<EthnicityModel>> ReadAsync(string species, string ethnicity, CancellationToken cancellationToken)
  {
    LineagePath path = new(species, ethnicity);
    EthnicityModel? model = await _lineageService.ReadEthnicityAsync(id: null, path, cancellationToken);
    return model is null ? NotFound() : Ok(model);
  }

  [HttpGet]
  public async Task<ActionResult<SearchResults<EthnicityModel>>> SearchAsync(string species, [FromQuery] SearchEthnicitiesParameters parameters, CancellationToken cancellationToken)
  {
    SearchEthnicitiesPayload payload = parameters.ToPayload();
    payload.Species = species;

    SearchResults<EthnicityModel> ethnicities = await _lineageService.SearchAsync(payload, cancellationToken);
    return Ok(ethnicities);
  }
}
