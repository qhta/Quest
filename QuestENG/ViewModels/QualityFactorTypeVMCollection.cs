namespace Quest;

/// <summary>
/// List of quality factor types.
/// </summary>
public class QualityFactorTypeVMCollection: ObservableList<QualityFactorTypeVM>
{
  private ProjectQualityVM Parent { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="DocumentQualityVMCollection"/> class with a list of <see cref="DocumentQuality"/>.
  /// </summary>
  /// <param name="parent">Parent view model</param>
  /// <param name="items">Collection of entities to add their view models.</param>
  public QualityFactorTypeVMCollection(ProjectQualityVM parent, IEnumerable<QualityFactorType> items) :
    base(items.Select(item => new QualityFactorTypeVM(item)))
  {
    Parent = parent;
  }

}