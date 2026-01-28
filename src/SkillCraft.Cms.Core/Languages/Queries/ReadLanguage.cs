using Krakenar.Contracts;
using Logitar.CQRS;
using SkillCraft.Cms.Core.Languages.Models;

namespace SkillCraft.Cms.Core.Languages.Queries;

internal record ReadLanguageQuery(Guid? Id, string? Slug) : IQuery<LanguageModel?>;

internal class ReadLanguageQueryHandler : IQueryHandler<ReadLanguageQuery, LanguageModel?>
{
  private readonly ILanguageQuerier _languageQuerier;

  public ReadLanguageQueryHandler(ILanguageQuerier languageQuerier)
  {
    _languageQuerier = languageQuerier;
  }

  public async Task<LanguageModel?> HandleAsync(ReadLanguageQuery query, CancellationToken cancellationToken)
  {
    Dictionary<Guid, LanguageModel> languages = new(capacity: 2);

    if (query.Id.HasValue)
    {
      LanguageModel? language = await _languageQuerier.ReadAsync(query.Id.Value, cancellationToken);
      if (language is not null)
      {
        languages[language.Id] = language;
      }
    }

    if (!string.IsNullOrWhiteSpace(query.Slug))
    {
      LanguageModel? language = await _languageQuerier.ReadAsync(query.Slug, cancellationToken);
      if (language is not null)
      {
        languages[language.Id] = language;
      }
    }

    if (languages.Count > 1)
    {
      throw TooManyResultsException<LanguageModel>.ExpectedSingle(languages.Count);
    }

    return languages.Values.SingleOrDefault();
  }
}
