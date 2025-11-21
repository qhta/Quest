namespace Qhta.MVVM;

/// <summary>
/// Structure used as a parameter to file open command.
/// </summary>
public record FileOpenData(string? Filename)
{
  /// <summary>
  /// Optional flag indicating whether the file is to be opened in read-only mode.
  /// </summary>
  public bool ReadOnly { get; set; }
}