namespace Quest;

/// <summary>
/// ViewModel for an assessed Project
/// </summary>
public class ProjectVM: ViewModel<Project>
{
  /// <summary>
  /// Mandatory constructor
  /// </summary>
  /// <param name="model"></param>
  public ProjectVM(Project model) : base(model)
  {
  }

  /// <summary>
  /// Entity identifier
  /// </summary>
  public int ID => Model.ID;

  /// <summary>
  /// Global Unique Identifier of the project
  /// </summary>
  public Guid Guid
  {
    [DebuggerStepThrough]
    get => Model.Guid;
    set
    {
      if (Model.Guid != value)
      {
        Model.Guid = value;
        NotifyPropertyChanged(nameof(Guid));
      }
    }
  }

  /// <summary>
  /// A title of the project
  /// </summary>
  public string? Title
  {
    [DebuggerStepThrough]
    get => Model.Title;
    set
    {
      if (Model.Title != value)
      {
        Model.Title = value;
        NotifyPropertyChanged(nameof(Title));
      }
    }
  }
}