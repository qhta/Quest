namespace Quest;

/// <summary>
/// Observable collection of <see cref="DocumentQualityVM"/> objects.
/// </summary>
public class DocumentQualityVMCollection : ObservableList<DocumentQualityVM>, IChangeable
{
  private ProjectQualityVM Parent { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="DocumentQualityVMCollection"/> class with a list of <see cref="DocumentQuality"/>.
  /// </summary>
  /// <param name="parent">Parent view model</param>
  /// <param name="items">Collection of entities to add their view models.</param>
  public DocumentQualityVMCollection(ProjectQualityVM parent,IEnumerable<DocumentQuality> items) : 
    base([])
  {
    Parent = parent;
    foreach (var item in items)
      Add(new DocumentQualityVM(parent, item));
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

  /// <summary>
  /// Evaluated value;
  /// </summary>
  public double? Value
  {
    [DebuggerStepThrough]
    get => _Value;
    set
    {
      if (_Value != value)
      {
        _Value = value;
        NotifyPropertyChanged(nameof(Value));
      }
    }
  }
  private double? _Value;

  /// <summary>
  /// Evaluates the weighted mean value of all items in the collection that have a defined value and a positive weight.
  /// </summary>
  /// <returns></returns>
  public double? EvaluateValue(bool refreshDeep)
  {
    double? valueSum = null;
    double? weightSum = null;
    foreach (var item in this)
    {
      double? itemValue = (refreshDeep) ? item.Evaluate() : item.Value;
      if (itemValue != null)
      {
        if (valueSum == null)
          valueSum = 0;
        if (weightSum == null)
          weightSum = 0;
        valueSum += itemValue;
        weightSum += 1;
      }
    }
    return weightSum > 0 ? valueSum / weightSum : null;
  }
}
