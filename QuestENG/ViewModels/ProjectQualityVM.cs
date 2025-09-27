using System.Collections.ObjectModel;

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
    DocumentQualities = new DocumentQualityCollection(this, model.DocumentQualities ?? []);
    ViewItems = new QuestItemsCollection(this);
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
  /// Individual grade scale definition within the project
  /// </summary>
  public QualityScaleVM Scale
  {
    [DebuggerStepThrough]
    get => _scale;
    set
    {
      if (_scale != value)
      {
        _scale = value;
        NotifyPropertyChanged(nameof(Scale));
        if (ViewItems.FirstOrDefault()?.Content is QualityScaleVM)
          ViewItems.RemoveAt(0);
        ViewItems.Add(new QuestItemViewModel{Header="Scale", Content=value});
      }
    }
  }
  private QualityScaleVM _scale = null!;

  /// <summary>
  /// Individual document qualities within the project
  /// </summary>
  public DocumentQualityCollection DocumentQualities
  {
    [DebuggerStepThrough]
    get => _documentQualities;
    set
    {
      if (_documentQualities != value)
      {
        _documentQualities = value;
        NotifyPropertyChanged(nameof(DocumentQualities));
      }
    }
  }
  private DocumentQualityCollection _documentQualities = null!;
  /// <summary>
  /// Determines whether the project quality is expanded (when used in TreeView).
  /// </summary>
  public bool IsExpanded
  {
    [DebuggerStepThrough]
    get => _isExpanded;
    set
    {
      if (_isExpanded != value)
      {
        _isExpanded = value;
        NotifyPropertyChanged(nameof(IsExpanded));
      }
    }
  }
  private bool _isExpanded;

  #region Loading State Properties
  /// <summary>
  /// Determines whether the workbook is currently in a loading state.
  /// </summary>
  public bool IsLoading
  {
    [DebuggerStepThrough]
    get => _isLoading;
    set
    {
      if (_isLoading != value)
      {
        _isLoading = value;
        NotifyPropertyChanged(nameof(IsLoading));
      }
    }
  }
  private bool _isLoading;

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
    get => _loadedCount;
    set
    {
      if (_loadedCount != value)
      {
        _loadedCount = value;
        NotifyPropertyChanged(nameof(LoadedCount));
      }
    }
  }
  private int _loadedCount;
  #endregion

  /// <summary>
  /// Collection of quest items. First item is quality scale, followed by phase qualities and document qualities.
  /// </summary>
  public QuestItemsCollection ViewItems { get; private set; }

};
