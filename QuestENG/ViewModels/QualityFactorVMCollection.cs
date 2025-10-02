namespace Quest;

/// <summary>
/// Observable collection of <see cref="QualityFactorVM"/> objects.
/// </summary>
public class QualityFactorVMCollection : ObservableList<QualityFactorVM>
{
  /// <summary>
  /// Parent view model
  /// </summary>
  public DocumentQualityVM Parent { get; set; }

  /// <summary>
  /// Initializes a new instance of the <see cref="QualityFactorVMCollection"/> class with a list of <see cref="QualityFactor"/>.
  /// </summary>
  /// <param name="parent">Parent view model</param>
  /// <param name="items">Collection of entities to add their view models.</param>
  public QualityFactorVMCollection(DocumentQualityVM parent,IEnumerable<QualityFactor> items) : 
    base(items.Select(item => new QualityFactorVM(parent, item)))
  {
    Parent = parent;
  }
}