namespace QuestWPF;

/// <summary>
/// Data class for file save operations.
/// </summary>
public record FileSaveData(MainWindow MainWindow, bool SaveAs = false)
{
  /// <summary>
  /// Gets the data object associated with the currently active view in the main window.
  /// </summary>
  public Object? DataObject => MainWindow.ActiveView?.DataContext;

  /// <summary>
  /// Filename to save the data to. If null or empty, a file dialog should be used to prompt the user.
  /// </summary>
  public string? Filename { get; set; }

}