using System.ComponentModel;

namespace Quest;

/// <summary>
/// Interface of view model for a quality object (project, document, etc.)
/// </summary>
public interface IQualityObjectVM: INotifyPropertyChanged
{

  /// <summary>
  /// Evaluates the value of the node.
  /// </summary>
  /// <returns>double value or null if evaluation is not possible</returns>
  public double? Evaluate();
}
