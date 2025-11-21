namespace Quest;

/// <summary>
/// ViewModel for a quest item in a collection
/// </summary>
public class QuestItemViewModel: IChangeable
{
  /// <summary>
  /// Header text for the quest item
  /// </summary>
  public string? Header { get; set; }
  /// <summary>
  /// Any content associated with the quest item
  /// </summary>
  public Object? Content { get; set; }


  /// <summary>
  /// Gets or sets a value indicating whether any item in the collection has been modified.
  /// </summary>
  /// <remarks>Setting this property to false updates the <c>IsChanged</c> state of all items in the collection to false.</remarks>
  public bool? IsChanged
  {
    get => (Content as IChangeable)?.IsChanged;
    set { if (Content is IChangeable content) content.IsChanged = value; }
  }
}
