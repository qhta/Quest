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
  /// Opens a file.
  /// A parameter should be of string or <see cref="IFileSaveData"/> type.
  /// </summary>
  Open,

  /// <summary>
  /// Save a file.
  /// A parameter should be of <see cref="IFileSaveData"/> type.
  /// </summary>
  Save,
}