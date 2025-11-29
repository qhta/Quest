namespace Qhta.MVVM;

/// <summary>
/// Data structure used as a parameter to <see cref="WindowCommands.OpenWindow"/> command.
/// </summary>
/// <param name="Content">Content of the window</param>
/// <param name="Name">Name of the window</param>
/// <param name="Title">Title of the window</param>
public record WindowOpenData(object Content, string Name, string? Title=null)
{
  /// <summary>
  /// An optional owner of the window to be opened.
  /// </summary>
  public object? Owner { get; set; }

  /// <summary>
  /// Optional class identifier for the window to be opened.
  /// </summary>
  public object? ClassID { get; set; }

  /// <summary>
  /// Optional X coordinate for the window to be opened.
  /// </summary>
  public int? Left { get; set; }

  /// <summary>
  /// Optional Y coordinate for the window to be opened.
  /// </summary>
  public int? Top { get; set; }

  /// <summary>
  /// Optional X size for the window to be opened.
  /// </summary>
  public int? Width { get; set; }

  /// <summary>
  /// Optional Y size for the window to be opened.
  /// </summary>
  public int? Height { get; set; }

  /// <summary>
  /// Initial state of the window to be opened. Can be an enum value of e.g. System.Windows.WindowState.
  /// </summary>
  public object? State { get; set; }
}