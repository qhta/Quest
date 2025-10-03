using System.Linq;

namespace Quest;

/// <summary>
/// Observable collection of <see cref="IQualityNodeVM"/> objects.
/// </summary>
public class QualityNodeVMCollection : ObservableList<IQualityNodeVM>
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
    base(items.Select<QualityNode, IQualityNodeVM>(item =>
    {
      if (item is QualityFactor qualityFactor)
      {
        return new QualityFactorVM(parent, qualityFactor);
      }
      if (item is QualityMetrics qualityMetrics)
      {
        return new QualityMetricsVM(parent, qualityMetrics);
      }
      if (item is QualityMeasure qualityMeasure)
      {
        return new QualityMeasureVM(parent, qualityMeasure);
      }
      throw new NotImplementedException("Invalid item type when creating ViewModel");
    }))
  {
    Parent = parent;
  }

  /// <summary>
  /// Inserts the specified item into the collection at the specified index setting its parent if necessary
  /// and subscribing to its PropertyChanged event.
  /// </summary>
  /// <param name="index">The zero-based index at which the item should be inserted.</param>
  /// <param name="item">The item to insert into the collection. Cannot be <see langword="null"/>.</param>
  protected override void InsertItem(int index, IQualityNodeVM item)
  {
    base.InsertItem(index, item);
    if (item.Parent != Parent)
      item.Parent = Parent;
    item.PropertyChanged += Item_PropertyChanged;
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
        Value = EvaluateValue(false);
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
    Value = weightSum > 0 ? valueSum / weightSum : null;
    return Value;
  }

}