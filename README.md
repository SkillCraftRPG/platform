# SkillCraftRPG Platform

A core application platform combining identity, content management, localization, and content materialization for public-facing sites and services.

## Migrations

This project is setup to use migrations. All the commands below must be executed in the solution directory.

### Create a migration

To create a new migration, execute the following command. Do not forget to provide a migration name!

```sh
dotnet ef migrations add <YOUR_MIGRATION_NAME> --context RulesContext --project src/SkillCraft.Cms.PostgreSQL --startup-project src/SkillCraft.Cms
```

### Remove a migration

To remove the latest unapplied migration, execute the following command.

```sh
dotnet ef migrations remove --context RulesContext --project src/SkillCraft.Cms.PostgreSQL --startup-project src/SkillCraft.Cms
```

### Generate a script

To generate a script, execute the following command. Do not forget to provide a source migration name!

```sh
dotnet ef migrations script <SOURCE_MIGRATION> --context RulesContext --project src/SkillCraft.Cms.PostgreSQL --startup-project src/SkillCraft.Cms
```
