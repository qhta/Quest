namespace QuestWPF;

/// <summary>
/// Data class for view copy operations.
/// </summary>
public record ViewCopyData(MainWindow MainWindow)
{
  /// <summary>
  /// Gets the data object associated with the currently active view in the main window.
  /// </summary>
  public Control? ActiveView => MainWindow.ActiveView;

}