namespace Quest;

/// <summary>
/// Static class providing filename related tools.
/// </summary>
public static class FilenameTools
{
  /// <summary>
  /// Creates a file filter string suitable for use with file dialogs, based on the specified description and file
  /// extensions.
  /// </summary>
  /// <remarks>The returned string is intended for use with the Filter property of file dialogs such as
  /// OpenFileDialog or SaveFileDialog. Each extension should include the leading dot (e.g., ".txt").</remarks>
  /// <param name="description">The display name or description for the file type (for example, "Text Files").</param>
  /// <param name="extensions">An array of file extension strings (including the dot, such as ".txt") to include in the filter. Cannot be null or
  /// empty.</param>
  /// <returns>A filter string in the format "Description (*.ext1;*.ext2)|*.ext1;*.ext2" that can be used with file dialog
  /// filters.</returns>
  public static string MakeFilterString(string description, params string[] extensions)
  {
    var extList = string.Join(";", extensions.Select(ext => $"*{ext}"));
    return $"{description} ({extList})|{extList}";
  }
}
