using Krakenar.Contracts.Search;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Cms.Core.Languages;
using SkillCraft.Cms.Core.Languages.Models;
using SkillCraft.Cms.Models.Parameters;

namespace SkillCraft.Cms.Controllers;

[ApiController]
[Route("api/rules/languages")]
public class LanguageController : ControllerBase
{
  private readonly ILanguageService _languageService;

  public LanguageController(ILanguageService languageService)
  {
    _languageService = languageService;
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<LanguageModel>> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    LanguageModel? language = await _languageService.ReadAsync(id, slug: null, cancellationToken);
    return language is null ? NotFound() : Ok(language);
  }

  [HttpGet("slug:{slug}")]
  public async Task<ActionResult<LanguageModel>> ReadAsync(string slug, CancellationToken cancellationToken)
  {
    LanguageModel? language = await _languageService.ReadAsync(id: null, slug, cancellationToken);
    return language is null ? NotFound() : Ok(language);
  }

  [HttpGet]
  public async Task<ActionResult<SearchResults<LanguageModel>>> SearchAsync([FromQuery] SearchLanguagesParameters parameters, CancellationToken cancellationToken)
  {
    SearchLanguagesPayload payload = parameters.ToPayload();
    SearchResults<LanguageModel> languages = await _languageService.SearchAsync(payload, cancellationToken);
    return Ok(languages);
  }
}
