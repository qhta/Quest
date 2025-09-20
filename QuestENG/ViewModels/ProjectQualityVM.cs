namespace Quest;

/// <summary>
/// ViewModel for a project quality assessment
/// </summary>
public class ProjectQualityVM : ViewModel<ProjectQuality>
{
  /// <summary>
  /// Mandatory constructor
  /// </summary>
  /// <param name="model"></param>
  public ProjectQualityVM(ProjectQuality model) : base(model)
  {
    RootNode = [this];
    DocumentQualities = new DocumentQualityCollection(model.DocumentQualities ?? []);
  }

  /// <summary>
  /// Root node for the quality tree (always this instance)
  /// </summary>
  public IEnumerable<ProjectQualityVM> RootNode { get; }

  /// <summary>
  /// Project title from the model
  /// </summary>
  public string? ProjectTitle
  {
    [DebuggerStepThrough]
    get => Model.ProjectName;
    set
    {
      if (Model.ProjectName != value)
      {
        Model.ProjectName = value;
        NotifyPropertyChanged(nameof(ProjectTitle));
      }
    }
  }

  /// <summary>
  /// Individual document qualities within the project
  /// </summary>
  public DocumentQualityCollection DocumentQualities { get; set; }

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
