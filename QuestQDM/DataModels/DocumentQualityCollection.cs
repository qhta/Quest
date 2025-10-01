namespace Quest;

/// <summary>
/// Observable collection of <see cref="DocumentQuality"/> objects.
/// </summary>
public class DocumentQualityCollection : ObservableList<DocumentQuality>
{
  private ProjectQuality Parent { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="DocumentQualityCollection"/> class with a list of <see cref="DocumentQuality"/>.
  /// </summary>
  /// <param name="parent">Parent view model</param>
  /// <param name="items">Collection of entities to add their view models.</param>
  public DocumentQualityCollection(ProjectQuality parent,IEnumerable<DocumentQuality> items) : base(items)
  {
    Parent = parent;
  }
}