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
  /// Evaluates both value and reliability.
  /// </summary>
  public void Evaluate()
  {
    bool refreshDeep = true;
    EvaluateValue(refreshDeep);
    EvaluateReliability(refreshDeep);
  }

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
      double? itemValue = (refreshDeep) ? item.EvaluateValue() : item.Value;
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

  /// <summary>
  /// Evaluated reliability;
  /// </summary>
  public double? Reliability
  {
    [DebuggerStepThrough]
    get => _Reliability;
    set
    {
      if (_Reliability != value)
      {
        _Reliability = value;
        NotifyPropertyChanged(nameof(Reliability));
      }
    }
  }
  private double? _Reliability;

  /// <summary>
  /// Evaluates the weighted mean Reliability of all items in the collection that have a defined Reliability and a positive weight.
  /// </summary>
  /// <returns></returns>
  public double? EvaluateReliability(bool refreshDeep)
  {
    double? ReliabilitySum = null;
    double? weightSum = null;
    foreach (var item in this)
    {
      double? itemReliability = (refreshDeep) ? item.EvaluateReliability() : item.Reliability;
      if (itemReliability != null)
      {
        if (ReliabilitySum == null)
          ReliabilitySum = 0;
        if (weightSum == null)
          weightSum = 0;
        ReliabilitySum += itemReliability;
        weightSum += 1;
      }
    }
    return weightSum > 0 ? ReliabilitySum / weightSum : null;
  }
}
