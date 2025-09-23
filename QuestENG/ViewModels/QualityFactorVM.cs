namespace Quest;

/// <summary>
/// ViewModel for a quality factor assessment
/// </summary>
public class QualityFactorVM : ViewModel<QualityFactor>
{
  /// <summary>
  /// Mandatory constructor
  /// </summary>
  /// <param name="model"></param>
  public QualityFactorVM(QualityFactor model) : base(model)
  {
    //DocumentQualities = new DocumentQualityCollection(this, model.DocumentQualities ?? []);
  }

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

  ///// <summary>
  ///// Individual document qualities within the project
  ///// </summary>
  //public DocumentQualityCollection DocumentQualities { get; set; }

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
