using Krakenar.Contracts.Search;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Cms.Core.Customizations;
using SkillCraft.Cms.Core.Customizations.Models;
using SkillCraft.Cms.Models.Parameters;

namespace SkillCraft.Cms.Controllers;

[ApiController]
[Route("api/customizations")]
public class CustomizationController : ControllerBase
{
  private readonly ICustomizationService _customizationService;

  public CustomizationController(ICustomizationService customizationService)
  {
    _customizationService = customizationService;
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<CustomizationModel>> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    CustomizationModel? customization = await _customizationService.ReadAsync(id, slug: null, cancellationToken);
    return customization is null ? NotFound() : Ok(customization);
  }

  [HttpGet("slug:{slug}")]
  public async Task<ActionResult<CustomizationModel>> ReadAsync(string slug, CancellationToken cancellationToken)
  {
    CustomizationModel? customization = await _customizationService.ReadAsync(id: null, slug, cancellationToken);
    return customization is null ? NotFound() : Ok(customization);
  }

  [HttpGet]
  public async Task<ActionResult<SearchResults<CustomizationModel>>> SearchAsync([FromQuery] SearchCustomizationsParameters parameters, CancellationToken cancellationToken)
  {
    SearchCustomizationsPayload payload = parameters.ToPayload();
    SearchResults<CustomizationModel> customizations = await _customizationService.SearchAsync(payload, cancellationToken);
    return Ok(customizations);
  }
}
