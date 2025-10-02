namespace Quest;

/// <summary>
/// ViewModel for a quality factor assessment
/// </summary>
public class QualityMetricsVM : ViewModel<QualityMetrics>, IQualityNodeVM
{
  /// <summary>
  /// Mandatory constructor
  /// </summary>
  /// <param name="parent">Required parent View Model</param>
  /// <param name="model">Required data entity</param>
  public QualityMetricsVM(IQualityObjectVM parent, QualityMetrics model) : base(model)
  {
    Parent = parent;
    Children = new QualityNodeVMCollection(this, model.Children ?? []);
  }

  
  /// <summary>
  /// Required parent view model
  /// </summary>
  public IQualityObjectVM Parent { get; set; }

  /// <summary>
  /// Gets the quality object associated with the model.
  /// </summary>
  public QualityObject QualityObject => Model;

  /// <summary>
  /// Level of the factor in the hierarchy (0 for root, 1 for factor)
  /// </summary>
  public int Level => Model.Level;

  /// <summary>
  /// Factor text from the model
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
  /// Weight of the factor  
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
  /// Gets the collection of child nodes associated with this node.
  /// </summary>
  /// <remarks>This property provides access to the hierarchical structure of nodes. It is read-only and cannot
  /// be null.</remarks>
  public QualityNodeVMCollection Children { get; }

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


};
