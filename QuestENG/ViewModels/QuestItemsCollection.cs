namespace Quest;

/// <summary>
/// Collection of quest items. First item is quality scale, followed by phase qualities and document qualities.
/// </summary>
public class QuestItemsCollection: ObservableList<QuestItemViewModel>
{
  private ProjectQualityVM Parent { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="QuestItemsCollection"/> class with parent.
  /// </summary>
  /// <param name="parent">Parent view model</param>
  public QuestItemsCollection(ProjectQualityVM parent) : base([])
  {
    Parent = parent;
  }
}