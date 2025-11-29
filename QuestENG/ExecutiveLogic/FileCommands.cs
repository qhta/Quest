namespace Qhta.MVVM;

/// <summary>
/// Represents a set of commands commonly used in file-based applications.
/// </summary>
/// <remarks>This enumeration defines basic commands such as <see cref="Open"/> and <see cref="Save"/>  that can
/// be used to represent actions in a Windows application. It is intended for use in  scenarios where a standardized set
/// of commands is needed.</remarks>
public enum FileCommands
{
  /// <summary>
  /// Open a file.
  /// </summary>
  Open,

  /// <summary>
  /// Save a file.
  /// </summary>
  Save,

  /// <summary>
  /// Save a file.
  /// </summary>
  SaveAs,

  /// <summary>
  /// Import a file.
  /// </summary>
  Import,

  /// <summary>
  /// Export a file.
  /// </summary>
  Export,

  /// <summary>
  /// Print a file.
  /// </summary>
  Print,

  /// <summary>
  /// Set options for a print job for a file.
  /// </summary>
  PrintSetup,

  /// <summary>
  /// Close a file.
  /// </summary>
  Close,
}