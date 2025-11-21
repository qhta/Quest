namespace Quest;

/// <summary>
/// List of quality factor types.
/// </summary>
public class QualityFactorTypeVMCollection: ObservableList<QualityFactorTypeVM>, IChangeable
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

  /// <summary>
  /// Gets or sets a value indicating whether any item in the collection has been modified.
  /// </summary>
  /// <remarks>Setting this property to false updates the <c>IsChanged</c> state of all items in the collection to false.</remarks>
  public bool? IsChanged
  {
    get => this.Any(g => g.IsChanged == true);
    set { if (value == false) this.ForEach(g => g.IsChanged = value); }
  }
}