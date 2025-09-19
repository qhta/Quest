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
  /// Project name from the model
  /// </summary>
  public string? ProjectName
  {
    [DebuggerStepThrough]
    get => Model.ProjectName;
    set
    {
      if (Model.ProjectName != value)
      {
        Model.ProjectName = value;
        NotifyPropertyChanged(nameof(ProjectName));
      }
    }
  }

  /// <summary>
  /// Individual document qualities within the project
  /// </summary>
  public DocumentQualityCollection DocumentQualities { get; set; }
};
