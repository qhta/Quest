namespace Quest;

/// <summary>
/// Collection of quest items. First item is quality scale, followed by phase qualities and document qualities.
/// </summary>
public class QuestItemsCollection: ObservableList<QuestItemViewModel>, IChangeable
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


  /// <summary>
  /// Gets or sets a value indicating whether any item in the collection has been modified.
  /// </summary>
  /// <remarks>Setting this property to false updates the <c>IsChanged</c> state of all items in the collection to false.</remarks>
  public bool? IsChanged
  {
    get => this.Any(g => g.IsChanged == true);
    set { if (value == false) this.ForEach(g => g.IsChanged = value); }
  }
}