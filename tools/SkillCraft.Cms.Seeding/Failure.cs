namespace SkillCraft.Cms.Seeding;

internal record Failure<T>(T Value, Exception Exception);
