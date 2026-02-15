using Krakenar.Core.Contents;
using Krakenar.Core.Contents.Events;
using AggregateEntity = Krakenar.EntityFrameworkCore.Relational.Entities.Aggregate;

namespace SkillCraft.Cms.Infrastructure.Entities;

internal class QuestEntity : AggregateEntity
{
  public int QuestId { get; private set; }
  public Guid Id { get; private set; }

  public bool IsPublished { get; private set; }

  public string Title { get; set; } = string.Empty;

  public QuestLogEntity? QuestLog { get; private set; }
  public int QuestLogId { get; private set; }
  public Guid QuestLogUid { get; private set; }

  public QuestGroupEntity? QuestGroup { get; private set; }
  public int? QuestGroupId { get; private set; }
  public Guid? QuestGroupUid { get; private set; }

  public int GrantedLevels { get; set; }
  public double ProgressRatio { get; set; }

  public string? HtmlContent { get; set; }
  public string? CompletedEntries { get; set; }
  public string? ActiveEntries { get; set; }

  public QuestEntity(ContentLocalePublished @event) : base(@event)
  {
    Id = new ContentId(@event.StreamId).EntityId;
  }

  private QuestEntity() : base()
  {
  }

  public void Publish(ContentLocalePublished @event)
  {
    Update(@event);

    IsPublished = true;
  }

  public void SetQuestGroup(QuestGroupEntity? group)
  {
    QuestGroup = group;
    QuestGroupId = group?.QuestGroupId;
    QuestGroupUid = group?.Id;
  }

  public void SetQuestLog(QuestLogEntity log)
  {
    QuestLog = log;
    QuestLogId = log.QuestLogId;
    QuestLogUid = log.Id;
  }

  public void Unpublish(ContentLocaleUnpublished @event)
  {
    Update(@event);

    IsPublished = false;
  }

  public override string ToString() => $"{Title} | {base.ToString()}";
}
