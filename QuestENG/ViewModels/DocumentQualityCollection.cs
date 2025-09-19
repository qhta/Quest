namespace Quest;

/// <summary>
/// Observable collection of <see cref="DocumentQualityVM"/> objects.
/// </summary>
public class DocumentQualityCollection : ObservableList<DocumentQualityVM>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="DocumentQualityCollection"/> class with a list of <see cref="DocumentQuality"/>.
  /// </summary>
  /// <param name="items"></param>
  public DocumentQualityCollection(IEnumerable<DocumentQuality> items) : base(items.Select(item => new DocumentQualityVM(item)))
  {
  }
}