using System.Linq;

namespace Quest;

/// <summary>
/// Observable collection of <see cref="IQualityNodeVM"/> objects.
/// </summary>
public class QualityNodeVMCollection : ObservableList<IQualityNodeVM>
{
  /// <summary>
  /// Parent view model
  /// </summary>
  public IQualityObjectVM Parent { get; set; }

  /// <summary>
  /// Initializing constructor.
  /// </summary>
  /// <param name="parent">Parent view model</param>
  /// <param name="items">Collection of entities to add their view models.</param>
  public QualityNodeVMCollection(IQualityObjectVM parent, IEnumerable<QualityNode> items) :
    base(items.Select<QualityNode, IQualityNodeVM>(item =>
    {
      if (item is QualityFactor qualityFactor)
      {
        return new QualityFactorVM(qualityFactor);
      }
      if (item is QualityMetrics qualityMetrics)
      {
        return new QualityMetricsVM(qualityMetrics);
      }
      if (item is QualityMeasure qualityMeasure)
      {
        return new QualityMeasureVM(qualityMeasure);
      }
      throw new NotImplementedException("Invalid item type when creating ViewModel");
    }))
  {
    Parent = parent;
  }
}