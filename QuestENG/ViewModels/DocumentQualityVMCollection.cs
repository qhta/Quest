namespace Quest;

/// <summary>
/// Observable collection of <see cref="DocumentQualityVM"/> objects.
/// </summary>
public class DocumentQualityVMCollection : ObservableList<DocumentQualityVM>
{
  private ProjectQualityVM Parent { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="DocumentQualityVMCollection"/> class with a list of <see cref="DocumentQuality"/>.
  /// </summary>
  /// <param name="parent">Parent view model</param>
  /// <param name="items">Collection of entities to add their view models.</param>
  public DocumentQualityVMCollection(ProjectQualityVM parent,IEnumerable<DocumentQuality> items) : 
    base(items.Select(item => new DocumentQualityVM(parent, item)))
  {
    Parent = parent;
  }
}