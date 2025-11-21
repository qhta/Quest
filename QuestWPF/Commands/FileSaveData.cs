namespace QuestWPF;

/// <summary>
/// Data class for file save operations.
/// </summary>
public record FileSaveData(MainWindow MainWindow): IFileSaveData
{
  /// <summary>
  /// Gets the data object associated with the currently active view in the main window.
  /// </summary>
  public Object? DataObject => MainWindow.ActiveView?.DataContext;

  /// <summary>
  /// Filename to save the data to. If null or empty, a file dialog should be used to prompt the user.
  /// </summary>
  public string? Filename { get; set; }

  /// <summary>
  /// Gets or sets a value indicating whether a dialog should be used for user interactions.
  /// </summary>
  public bool UseDialog { get; set; }

}