using Krakenar.Contracts.Logging;
using Krakenar.Contracts.Settings;
using Krakenar.Core;
using Krakenar.Core.Caching;
using Krakenar.Core.Realms;
using Krakenar.Core.Tokens;
using Krakenar.Core.Users;
using Logitar.EventSourcing;
using Configuration = Krakenar.Contracts.Configurations.Configuration;
using Realm = Krakenar.Contracts.Realms.Realm;
using User = Krakenar.Contracts.Users.User;

namespace SkillCraft.Cms.Seeding;

internal class SeedingApplicationContext : IApplicationContext
{
  private readonly ICacheService _cacheService;

  public SeedingApplicationContext(ICacheService cacheService)
  {
    _cacheService = cacheService;
  }

  protected virtual Configuration Configuration => _cacheService.Configuration ?? throw new InvalidOperationException("The configuration was not found in the cache.");

  public User? User { get; set; }
  public ActorId? ActorId
  {
    get
    {
      if (User is null)
      {
        return null;
      }

      RealmId? realmId = User.Realm is null ? null : new(User.Realm.Id);
      UserId userId = new(User.Id, realmId);
      return new ActorId(userId.Value);
    }
  }

  public string BaseUrl { get; set; } = "seeding";

  public Realm? Realm { get; set; }
  public RealmId? RealmId => Realm is null ? null : new RealmId(Realm.Id);

  public Secret Secret => new(Realm?.Secret ?? Configuration.Secret ?? string.Empty);
  public IUniqueNameSettings UniqueNameSettings => Realm?.UniqueNameSettings ?? Configuration.UniqueNameSettings;
  public IPasswordSettings PasswordSettings => Realm?.PasswordSettings ?? Configuration.PasswordSettings;
  public bool RequireUniqueEmail => Realm?.RequireUniqueEmail ?? false;
  public bool RequireConfirmedAccount => Realm?.RequireConfirmedAccount ?? false;

  public ILoggingSettings LoggingSettings => Configuration.LoggingSettings;
}
