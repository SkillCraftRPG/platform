using Krakenar.Core.Contents;
using Krakenar.Core.Fields;

namespace SkillCraft.Cms.Infrastructure.Contents;

public static class ContentExtensions
{
  public static IReadOnlyCollection<string> GetSelect(this ContentLocale locale, Guid fieldId, IReadOnlyCollection<string>? defaultValue = null)
  {
    return locale.TryGetSelect(fieldId) ?? defaultValue ?? [];
  }
  public static IReadOnlyCollection<string>? TryGetSelect(this ContentLocale locale, Guid fieldId)
  {
    return locale.FieldValues.TryGetValue(fieldId, out FieldValue? value)
      ? JsonSerializer.Deserialize<IReadOnlyCollection<string>>(value.Value)
      : null;
  }

  public static string GetString(this ContentLocale locale, Guid fieldId, string defaultValue = "")
  {
    return locale.TryGetString(fieldId) ?? defaultValue;
  }
  public static string? TryGetString(this ContentLocale locale, Guid fieldId)
  {
    return locale.FieldValues.TryGetValue(fieldId, out FieldValue? value) ? value.Value : null;
  }
}
