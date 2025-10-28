namespace Quest;

/// <summary>
/// Observable collection of <see cref="QualityFactor"/> objects.
/// </summary>
[XmlCollection]
[HideInheritedMembers]
public class QualityFactorCollection : ObservableList<QualityFactor>
{
  /// <summary>
  /// Parent view model
  /// </summary>
  [XmlIgnore]
  public DocumentQuality Parent { get; set; }

  /// <summary>
  /// Default constructor.
  /// </summary>
  public QualityFactorCollection()
  {
    Parent = null!;
  }

  /// <summary>
  /// Default constructor.
  /// </summary>
  /// <param name="parent">Parent view model</param>
  public QualityFactorCollection(DocumentQuality parent) : base()
  {
    Parent = parent;
  }

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