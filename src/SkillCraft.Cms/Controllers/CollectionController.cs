using Microsoft.AspNetCore.Mvc;
using SkillCraft.Cms.Core.Collections;
using SkillCraft.Cms.Core.Collections.Models;

namespace SkillCraft.Cms.Controllers;

[ApiController]
[Route("api/collections")]
public class CollectionController : ControllerBase
{
  private readonly ICollectionService _collectionService;

  public CollectionController(ICollectionService collectionService)
  {
    _collectionService = collectionService;
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<CollectionModel>> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    CollectionModel? collection = await _collectionService.ReadAsync(id, slug: null, cancellationToken);
    return collection is null ? NotFound() : Ok(collection);
  }

  [HttpGet("slug:{slug}")]
  public async Task<ActionResult<CollectionModel>> ReadAsync(string slug, CancellationToken cancellationToken)
  {
    CollectionModel? collection = await _collectionService.ReadAsync(id: null, slug, cancellationToken);
    return collection is null ? NotFound() : Ok(collection);
  }
}
