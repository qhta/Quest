namespace QuestIMP;

/// <summary>
/// ViewModel for <see cref="WorkbookInfo"/>.
/// </summary>
public class WorkbookInfoVM: ViewModel<WorkbookInfo>
{
  /// <summary>
  /// Default constructor creating an empty model.
  /// </summary>
  public WorkbookInfoVM() : this(new WorkbookInfo())
  {

  }

  /// <summary>
  /// Mandatory constructor with model parameter.
  /// </summary>
  /// <param name="model"></param>
  public WorkbookInfoVM(WorkbookInfo model) : base(model)
  {
    Worksheets = new WorksheetInfoCollection(Model.Worksheets);
  }

  /// <summary>
  /// Represents the file name of the workbook.
  /// </summary>
  public string? FileName
  {
    get => Model.FileName;
    set
    {
      if (Model.FileName != value)
      {
        Model.FileName = value;
        NotifyPropertyChanged(nameof(FileName));
      }
    }
  }

  /// <summary>
  /// Represents the project title of the workbook.
  /// </summary>
  public string? ProjectTitle
  {
    get => Model.ProjectTitle;
    set
    {
      if (Model.ProjectTitle != value)
      {
        Model.ProjectTitle = value;
        NotifyPropertyChanged(nameof(ProjectTitle));
      }
    }
  }

  /// <summary>
  /// Collection of worksheet information associated with the workbook.
  /// </summary>
  public WorksheetInfoCollection Worksheets { get; private set; }
}