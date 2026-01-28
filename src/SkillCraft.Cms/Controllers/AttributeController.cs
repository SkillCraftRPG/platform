using Krakenar.Contracts.Search;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Cms.Core.Attributes;
using SkillCraft.Cms.Core.Attributes.Models;
using SkillCraft.Cms.Models.Parameters;

namespace SkillCraft.Cms.Controllers;

[ApiController]
[Route("api/attributes")]
public class AttributeController : ControllerBase
{
  private readonly IAttributeService _attributeService;

  public AttributeController(IAttributeService attributeService)
  {
    _attributeService = attributeService;
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<AttributeModel>> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    AttributeModel? attribute = await _attributeService.ReadAsync(id, slug: null, cancellationToken);
    return attribute is null ? NotFound() : Ok(attribute);
  }

  [HttpGet("slug:{slug}")]
  public async Task<ActionResult<AttributeModel>> ReadAsync(string slug, CancellationToken cancellationToken)
  {
    AttributeModel? attribute = await _attributeService.ReadAsync(id: null, slug, cancellationToken);
    return attribute is null ? NotFound() : Ok(attribute);
  }

  [HttpGet]
  public async Task<ActionResult<SearchResults<AttributeModel>>> SearchAsync([FromQuery] SearchAttributesParameters parameters, CancellationToken cancellationToken)
  {
    SearchAttributesPayload payload = parameters.ToPayload();
    SearchResults<AttributeModel> attributes = await _attributeService.SearchAsync(payload, cancellationToken);
    return Ok(attributes);
  }
}
