namespace Quest;

/// <summary>
/// Observable collection of <see cref="QualityFactorVM"/> objects.
/// </summary>
public class QualityFactorVMCollection : QualityNodeVMCollection<QualityFactorVM>, IChangeable
{
  /// <summary>
  /// Parent view model
  /// </summary>
  public new DocumentQualityVM Parent { get => (DocumentQualityVM)base.Parent; set => base.Parent = value; }

  /// <summary>
  /// Initializes a new instance of the <see cref="QualityFactorVMCollection"/> class with a list of <see cref="QualityFactor"/>.
  /// </summary>
  /// <param name="parent">Parent view model</param>
  /// <param name="items">Collection of entities to add their view models.</param>
  public QualityFactorVMCollection(DocumentQualityVM parent,IEnumerable<QualityFactor> items) :
    base(parent, [])
  {
    foreach (var item in items)
      Add(new QualityFactorVM(parent, this, item));
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