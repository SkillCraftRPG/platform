using Krakenar.Core;
using Krakenar.Core.Contents;
using Krakenar.Core.Fields;
using Krakenar.Core.Settings;

namespace SkillCraft.Cms.Infrastructure.Contents;

[Trait(Traits.Category, Categories.Unit)]
public class ContentExtensionsTests
{
  private readonly UniqueNameSettings _uniqueNameSettings = new();

  [Fact(DisplayName = "GetSelect: it should return the default value when not found.")]
  public void Given_NotFound_When_GetSelect_Then_DefaultReturned()
  {
    ContentLocale locale = new(new UniqueName(_uniqueNameSettings, "Dexterity"));
    Assert.Empty(locale.FieldValues);

    string[] defaultValue = ["N/A"];
    Assert.True(defaultValue.SequenceEqual(locale.GetSelect(Guid.Empty, defaultValue)));
  }

  [Fact(DisplayName = "GetSelect: it should return the value found.")]
  public void Given_Found_When_GetSelect_Then_ValueReturned()
  {
    Guid fieldId = Guid.NewGuid();
    string[] values = ["Physical"];

    ContentLocale locale = new(new UniqueName(_uniqueNameSettings, "Dexterity"), fieldValues: new Dictionary<Guid, FieldValue>
    {
      [fieldId] = new FieldValue(JsonSerializer.Serialize(values))
    });
    Assert.NotEmpty(locale.FieldValues);

    IReadOnlyCollection<string> result = locale.GetSelect(fieldId);
    Assert.True(values.SequenceEqual(result));
  }

  [Fact(DisplayName = "GetString: it should return the default value when not found.")]
  public void Given_NotFound_When_GetString_Then_DefaultReturned()
  {
    ContentLocale locale = new(new UniqueName(_uniqueNameSettings, "Dexterity"));
    Assert.Empty(locale.FieldValues);

    string defaultValue = "dexterity";
    Assert.Equal(defaultValue, locale.GetString(Guid.Empty, defaultValue));
  }

  [Fact(DisplayName = "GetString: it should return the value found.")]
  public void Given_Found_When_GetString_Then_ValueReturned()
  {
    Guid fieldId = Guid.NewGuid();
    string value = "adresse";

    ContentLocale locale = new(new UniqueName(_uniqueNameSettings, "Dexterity"), fieldValues: new Dictionary<Guid, FieldValue>
    {
      [fieldId] = new FieldValue(value)
    });
    Assert.NotEmpty(locale.FieldValues);

    Assert.Equal(value, locale.GetString(fieldId));
  }

  [Fact(DisplayName = "TryGetSelect: it should return null when not found.")]
  public void Given_NotFound_When_TryGetSelect_Then_NullReturned()
  {
    ContentLocale locale = new(new UniqueName(_uniqueNameSettings, "Dexterity"));
    Assert.Empty(locale.FieldValues);
    Assert.Null(locale.TryGetSelect(Guid.Empty));
  }

  [Fact(DisplayName = "TryGetSelect: it should return the value found.")]
  public void Given_Found_When_TryGetSelect_Then_ValueReturned()
  {
    Guid fieldId = Guid.NewGuid();
    string[] values = ["Physical"];

    ContentLocale locale = new(new UniqueName(_uniqueNameSettings, "Dexterity"), fieldValues: new Dictionary<Guid, FieldValue>
    {
      [fieldId] = new FieldValue(JsonSerializer.Serialize(values))
    });
    Assert.NotEmpty(locale.FieldValues);

    IReadOnlyCollection<string>? result = locale.TryGetSelect(fieldId);
    Assert.NotNull(result);
    Assert.True(values.SequenceEqual(result));
  }

  [Fact(DisplayName = "TryGetString: it should return null when not found.")]
  public void Given_NotFound_When_TryGetString_Then_NullReturned()
  {
    ContentLocale locale = new(new UniqueName(_uniqueNameSettings, "Dexterity"));
    Assert.Empty(locale.FieldValues);
    Assert.Null(locale.TryGetString(Guid.Empty));
  }

  [Fact(DisplayName = "TryGetString: it should return the value found.")]
  public void Given_Found_When_TryGetString_Then_ValueReturned()
  {
    Guid fieldId = Guid.NewGuid();
    string value = "adresse";

    ContentLocale locale = new(new UniqueName(_uniqueNameSettings, "Dexterity"), fieldValues: new Dictionary<Guid, FieldValue>
    {
      [fieldId] = new FieldValue(value)
    });
    Assert.NotEmpty(locale.FieldValues);

    Assert.Equal(value, locale.TryGetString(fieldId));
  }
}
