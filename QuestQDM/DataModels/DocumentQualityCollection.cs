using System.Xml.Serialization;

namespace Quest;

/// <summary>
/// Observable collection of <see cref="DocumentQuality"/> objects.
/// </summary>
[XmlCollection]
[HideInheritedMembers]
public class DocumentQualityCollection : ObservableList<DocumentQuality>
{
  /// <summary>
  /// Parent project quality
  /// </summary>
  [XmlIgnore]
  public ProjectQuality Parent { get; set; }

  /// <summary>
  /// Default constructor.
  /// </summary>
  public DocumentQualityCollection()
  {
    Parent = null!;
  }

  /// <summary>
  /// Default constructor.
  /// </summary>
  /// <param name="parent">Parent view model</param>
  public DocumentQualityCollection(ProjectQuality parent) : base()
  {
    Parent = parent;
  }

  /// <summary>
  /// Initializing constructor.
  /// </summary>
  /// <param name="parent">Parent view model</param>
  /// <param name="items">Collection of entities to add their view models.</param>
  public DocumentQualityCollection(ProjectQuality parent,IEnumerable<DocumentQuality> items) : base(items)
  {
    Parent = parent;
  }
}