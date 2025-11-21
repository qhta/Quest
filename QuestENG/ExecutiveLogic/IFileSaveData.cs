using Microsoft.Extensions.DependencyModel;

namespace Qhta.MVVM;

/// <summary>
/// Structure used as a parameter to file save command.
/// </summary>
public interface IFileSaveData
{
  /// <summary>
  /// Data object to save
  /// </summary>
  public object? DataObject { get; }

  /// <summary>
  /// Filename to save to. If null, a file dialog should be shown.
  /// </summary>
  public string? Filename { get; }

  /// <summary>
  /// Optional flag indicating whether to perform "Save As" operation.
  /// </summary>
  public bool UseDialog { get; }
}