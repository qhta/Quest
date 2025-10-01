using System.ComponentModel;

namespace Quest;

/// <summary>
/// Interface of view model for a quality object (project, document, etc.)
/// </summary>
public interface IQualityObjectVM: INotifyPropertyChanged
{
  /// <summary>
  /// Gets the underlying quality object model.
  /// </summary>
  public QualityObject QualityObject { get; }
}
