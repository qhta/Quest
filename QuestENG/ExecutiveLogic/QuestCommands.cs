namespace Quest;

/// <summary>
/// Enumeration of command identifiers used in the Quest application.
/// </summary>
public enum QuestCommands
{
  /// <summary>
  /// Imports data from a spreadsheet file into the application.
  /// </summary>
  /// <remarks>This method supports common spreadsheet formats such as .xlsx and .csv. Ensure that the file is
  /// not open in another application to avoid access issues.</remarks>
  Import,
}