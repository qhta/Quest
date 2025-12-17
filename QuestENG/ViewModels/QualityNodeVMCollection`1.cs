namespace Quest;

/// <summary>
/// Observable collection of <see cref="IQualityNodeVM"/> objects.
/// </summary>
public class QualityNodeVMCollection<T> : ObservableList<T> where T : IQualityNodeVM
{
  /// <summary>
  /// Parent view model
  /// </summary>
  public IQualityObjectVM Parent { get; set; }

  /// <summary>
  /// Initializing constructor.
  /// </summary>
  /// <param name="parent">Parent view model</param>
  /// <param name="items">Collection of entities to add their view models.</param>
  public QualityNodeVMCollection(IQualityObjectVM parent, IEnumerable<QualityNode> items) :
    base([])
  {
    Parent = parent;
    CollectionChanged += QualityNodeVMCollection_CollectionChanged;
    foreach (var item in items)
    {
      if (item is QualityFactor qualityFactor)
        Add((T)(object)new QualityFactorVM(parent, this, qualityFactor));
      else if (item is QualityMetrics qualityMetrics)
        Add((T)(object)new QualityMetricsVM(parent, this, qualityMetrics));
      else if (item is QualityMeasure qualityMeasure)
        Add((T)(object)new QualityMeasureVM(parent, this, qualityMeasure));
      else throw new NotImplementedException("Invalid item type when creating ViewModel");
    }
  }

  /// <summary>
  /// If new item is added, its parent is set to the collection's parent and its PropertyChanged event is subscribed to.
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  /// <exception cref="NotImplementedException"></exception>
  private void QualityNodeVMCollection_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
  {
    if (e.NewItems != null)
    {
      foreach (T item in e.NewItems)
      {
        if (item.Parent != Parent)
          item.Parent = Parent;
        item.PropertyChanged += Item_PropertyChanged;
      }
    }
  }

  private bool _isRefreshing;

  /// <summary>
  /// If an item's PropertyChanged event signals a change of the "Value" property,
  /// the collection will be notified to refresh its state.
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  private void Item_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
  {
    if (!_isRefreshing)
      if (e.PropertyName == "Value" || e.PropertyName == "Weight")
        Evaluate();
  }

  /// <summary>
  /// Evaluates both value and reliability.
  /// </summary>
  public void Evaluate()
  {
    EvaluateValue(false);
    EvaluateReliability(false);

  }
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
      if (itemValue != null && itemValue != 0)
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
    Value = weightSum > 0 ? valueSum / weightSum : null;
    return Value;
  }

  /// <summary>
  /// Reliability of grades in the collection.
  /// </summary>
  public double? Reliability
  {
    [DebuggerStepThrough]
    get
    {
      if (_isRefreshing)
        _Reliability = EvaluateReliability(true);
      return _Reliability;
    }
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
  /// Evaluates the percent of all items in the collection that have a defined grade.
  /// </summary>
  /// <returns></returns>
  public double? EvaluateReliability(bool refreshDeep)
  {
    _isRefreshing = refreshDeep;
    double? valueSum = null;
    int count = this.Count;
    foreach (var item in this)
    {
      double? itemValue = (refreshDeep) ? item.EvaluateReliability() : item.Reliability;
      if (itemValue != null && itemValue != 0)
      {
        if (valueSum == null)
          valueSum = 0;
        valueSum += itemValue;
      }
    }
    _isRefreshing = false;
    Reliability = count > 0 ? valueSum / count : null;
    return Reliability;
  }
}