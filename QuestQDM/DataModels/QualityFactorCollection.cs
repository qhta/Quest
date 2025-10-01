namespace Quest;

/// <summary>
/// Observable collection of <see cref="QualityFactor"/> objects.
/// </summary>
public class QualityFactorCollection : ObservableList<QualityFactor>
{
  /// <summary>
  /// Parent view model
  /// </summary>
  public DocumentQuality Parent { get; set; }

  /// <summary>
  /// Initializing constructor.
  /// </summary>
  /// <param name="parent">Parent view model</param>
  /// <param name="items">Collection of entities to add their view models.</param>
  public QualityFactorCollection(DocumentQuality parent, IEnumerable<QualityFactor> items) : base(items)
  {
    Parent = parent;
  }
}