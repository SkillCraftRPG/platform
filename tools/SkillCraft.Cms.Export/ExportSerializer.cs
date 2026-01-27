namespace SkillCraft.Cms.Export;

internal static class ExportSerializer
{
  private static readonly JsonSerializerOptions _serializerOptions = new();
  static ExportSerializer()
  {
    _serializerOptions.Converters.Add(new JsonStringEnumConverter());
    _serializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
    _serializerOptions.WriteIndented = true;
  }

  public static T? Deserialize<T>(string json) => JsonSerializer.Deserialize<T>(json, _serializerOptions);
  public static string Serialize<T>(T value) => JsonSerializer.Serialize(value, _serializerOptions);
}

// TODO(fpion): refactor
