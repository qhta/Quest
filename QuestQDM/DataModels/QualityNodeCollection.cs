namespace Quest;

/// <summary>
/// Observable collection of <see cref="QualityNode"/> objects.
/// </summary>
[XmlCollection]
[HideInheritedMembers]
public class QualityNodeCollection : ObservableList<QualityNode>
{
  /// <summary>
  /// Parent view model
  /// </summary>
  [XmlIgnore]
  public QualityObject Parent { get; set; }

  /// <summary>
  /// Initializes a new instance of the <see cref="QualityNodeCollection"/> class.
  /// </summary>
  /// <remarks>This constructor initializes the collection with a default state. The <see cref="Parent"/>
  /// property must be set after initialization to ensure proper usage.</remarks>
  public QualityNodeCollection()
  {
    Parent = null!;
  }

  /// <summary>
  /// Initializing constructor.
  /// </summary>
  /// <param name="parent">Parent view model</param>
  public QualityNodeCollection(QualityObject parent) : base()
  {
    Parent = parent;
  }

  /// <summary>
  /// Initializing constructor.
  /// </summary>
  /// <param name="parent">Parent view model</param>
  /// <param name="items">Collection of entities to add their view models.</param>
  public QualityNodeCollection(DocumentQuality parent, IEnumerable<QualityNode> items) : base(items)
  {
    Parent = parent;
  }
}