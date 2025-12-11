namespace Quest;

/// <summary>
/// Generalized class for QualityFactorVM, QualityMetricsVM, and QualityMeasureVM
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class QualityNodeVM<T>: ViewModel<T>, IQualityNodeVM where T: QualityNode
{
  /// <summary>
  /// Passes constructor call to base class
  /// </summary>
  /// <param name="model"></param>
  protected QualityNodeVM(T model) : base(model)
  {
  }

  /// <summary>
  /// Required parent view model
  /// </summary>
  public IQualityObjectVM? Parent
  {
    get => _parent;
    set
    {
      if (_parent != value)
      {
        _parent = value;
        NotifyPropertyChanged(nameof(Parent));
      }
    }
  }
  private IQualityObjectVM? _parent;

  /// <summary>
  /// Collection to which this node belongs.
  /// </summary>
  public IList? Collection { get; set; }

  /// <summary>
  /// Access to the model's factor type
  /// </summary>
  public abstract QualityFactorType? FactorType { get; set; }

  /// <summary>
  /// Level of the node in the hierarchy (0 for root, 1 for factor)
  /// </summary>
  public int Level => Model.Level;

  /// <summary>
  /// Ordering number string ended with a dot.
  /// </summary>
  public string? Numbering
  {
    get
    {
      var text = "";
      if (Parent is IQualityNodeVM parentNode)
        text = parentNode.Numbering;
      var number = (Collection?.IndexOf(this) ?? 0) + 1;
      text += number + ".";
      return text;
    }
  }

  /// <summary>
  /// Text from the model
  /// </summary>
  public string? Text
  {
    [DebuggerStepThrough]
    get => Model.Text;
    set
    {
      if (Model.Text != value)
      {
        Model.Text = value;
        NotifyPropertyChanged(nameof(Text));
      }
    }
  }

  /// <summary>
  /// Text from the model preceding with ordering number.
  /// </summary>
  public string? TextWithNumbering
  {
    [DebuggerStepThrough]
    get
    {
      var text = Model.Text;
      var numbering = Numbering;
      if (numbering != null)
        text = numbering + " " + text;
      return text;
    }
  }

  /// <summary>
  /// Gets a display name for the factor.
  /// </summary>
  public abstract string? DisplayName { get; }

  /// <summary>
  /// Gets a display name for the factor with numbering.
  /// </summary>
  public string? DisplayNameWithNumbering
  {
    [DebuggerStepThrough]
    get
    {
      var text = DisplayName;
      var numbering = Numbering;
      if (numbering != null)
        text = numbering + " " + text;
      return text;
    }
  }

  /// <summary>
  /// Gets the background color for display purposes.
  /// </summary>
  public abstract string? BackgroundColor { get; }

  /// <summary>
  /// Weight of the value.  
  /// </summary>
  public int Weight
  {
    [DebuggerStepThrough]
    get => Model.Weight;
    set
    {
      if (Model.Weight != value)
      {
        Model.Weight = value;
        NotifyPropertyChanged(nameof(Weight));
      }
    }
  }

  /// <summary>
  /// Value of the assessment
  /// </summary>
  public double? Value
  {
    [DebuggerStepThrough]
    get => Model.Value;
    set
    {
      if (Model.Value != value)
      {
        Model.Value = value;
        NotifyPropertyChanged(nameof(Value));
      }
    }
  }

  /// <summary>
  /// Comment added to assessment
  /// </summary>
  public string? Comment
  {
    [DebuggerStepThrough]
    get => Model.Comment;
    set
    {
      if (Model.Comment != value)
      {
        Model.Comment = value;
        NotifyPropertyChanged(nameof(Comment));
      }
    }
  }

  /// <summary>
  /// Evaluates the value of the children collection.
  /// </summary>
  /// <returns>double value or null if evaluation is not possible</returns>
  public abstract double? EvaluateValue();

  #region Loading State Properties
  /// <summary>
  /// Determines whether the workbook is currently in a loading state.
  /// </summary>
  public bool IsLoading
  {
    [DebuggerStepThrough]
    get => _IsLoading;
    set
    {
      if (_IsLoading != value)
      {
        _IsLoading = value;
        NotifyPropertyChanged(nameof(IsLoading));
      }
    }
  }
  private bool _IsLoading;

  /// <summary>
  /// Determines the count of worksheets to load.
  /// </summary>
  public int TotalCount
  {
    [DebuggerStepThrough]
    get => _totalCount;
    set
    {
      if (_totalCount != value)
      {
        _totalCount = value;
        NotifyPropertyChanged(nameof(TotalCount));
      }
    }
  }
  private int _totalCount;

  /// <summary>
  /// Currently loaded worksheets count
  /// </summary>
  public int LoadedCount
  {
    [DebuggerStepThrough]
    get => _LoadedCount;
    set
    {
      if (_LoadedCount != value)
      {
        _LoadedCount = value;
        NotifyPropertyChanged(nameof(LoadedCount));
      }
    }
  }
  private int _LoadedCount;
  #endregion
}