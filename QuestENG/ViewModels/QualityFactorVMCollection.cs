namespace Quest;

/// <summary>
/// Observable collection of <see cref="QualityFactorVM"/> objects.
/// </summary>
public class QualityFactorVMCollection : ObservableList<QualityFactorVM>, IChangeable
{
  /// <summary>
  /// Parent view model
  /// </summary>
  public DocumentQualityVM Parent { get; set; }

  /// <summary>
  /// Initializes a new instance of the <see cref="QualityFactorVMCollection"/> class with a list of <see cref="QualityFactor"/>.
  /// </summary>
  /// <param name="parent">Parent view model</param>
  /// <param name="items">Collection of entities to add their view models.</param>
  public QualityFactorVMCollection(DocumentQualityVM parent,IEnumerable<QualityFactor> items) : 
    base([])
  {
    foreach (var item in items)
      Add(new QualityFactorVM(parent, this, item));
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

  private bool _isRefreshing = false;
  /// <summary>
  /// Weighted mean value of items.
  /// </summary>
  public double? Value
  {
    [DebuggerStepThrough]
    get
    {
      if (_isRefreshing)
        _Value = EvaluateValue(true);
      return _Value;
    }
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
    _isRefreshing = refreshDeep;
    double? valueSum = null;
    double? weightSum = null;
    foreach (var item in this)
    {
      double? itemValue = (refreshDeep) ? item.EvaluateValue() : item.Value;
      if (itemValue != null)
      {
        if (valueSum == null)
          valueSum = 0;
        if (weightSum == null)
          weightSum = 0;
        valueSum += itemValue * item.Weight;
        weightSum += item.Weight;
      }
    }
    _isRefreshing = false;
    return weightSum > 0 ? valueSum / weightSum : null;
  }

}