namespace Quest;

/// <summary>
/// ViewModel for a quest item in a collection
/// </summary>
public class QuestItemViewModel
{
  /// <summary>
  /// Header text for the quest item
  /// </summary>
  public string? Header { get; set; }
  /// <summary>
  /// Any content associated with the quest item
  /// </summary>
  public object? Content { get; set; }
}