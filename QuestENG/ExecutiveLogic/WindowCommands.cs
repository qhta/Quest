namespace Qhta.MVVM;

/// <summary>
/// Represents a set of commands commonly used in Windows-based applications.
/// </summary>
/// <remarks>This enumeration defines basic commands such as <see cref="OpenWindow"/> and <see cref="Close"/>  that can
/// be used to represent actions in a Windows application. It is intended for use in  scenarios where a standardized set
/// of commands is needed.</remarks>
public enum WindowCommands
{
  /// <summary>
  /// Opens a window.
  /// A parameter should be of <see cref="WindowOpenData"/> type.
  /// </summary>
  OpenWindow,

  /// <summary>
  /// Closes a window.
  /// A parameter should be a window instance.
  /// </summary>
  Close,
}