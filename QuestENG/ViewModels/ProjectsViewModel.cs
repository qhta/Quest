namespace Quest;

/// <summary>
/// Singleton ViewModel that holds a ProjectsCollection.
/// </summary>
public class ProjectsViewModel : ViewModel
{
  /// <summary>
  /// Private constructor to enforce singleton pattern.
  /// </summary>
  private ProjectsViewModel()
  {
    // Initialize the ProjectsCollection with an empty list or fetch from a data source.
    Projects = new ProjectVMCollection(new List<Project>());
  }

  readonly QuestRdmDbContext dbContext = new QuestRdmDbContext();

  /// <summary>
  /// Gets the singleton instance of the ProjectsViewModel.
  /// </summary>
  public static ProjectsViewModel Instance => _instance.Value;
  private static readonly Lazy<ProjectsViewModel> _instance = new(() => new ProjectsViewModel());

  /// <summary>
  /// Collection of all projects.
  /// </summary>
  public ProjectVMCollection Projects
  {
    [DebuggerStepThrough]
    get => _Projects;
    set
    {
      if (_Projects != value)
      {
        _Projects = value;
        NotifyPropertyChanged(nameof(Projects));
      }
    }
  }
  private ProjectVMCollection _Projects = new ProjectVMCollection([]);

  /// <summary>
  /// Method to load projects from the QuestRDM.
  /// </summary>
  public void LoadProjects()
  {
    // Fetch projects from the database and populate the ProjectsCollection.
    var projects = dbContext.Projects.ToList();
    Projects = new ProjectVMCollection(projects);
  }

  /// <summary>
  /// Selected project in projects list.
  /// </summary>
  public ProjectVM? SelectedProject
  {
    get => _selectedProject;
    set
    {
      _selectedProject = value;
      NotifyPropertyChanged(nameof(SelectedProject));
    }
  }
  private ProjectVM? _selectedProject;

  /// <summary>
  /// Editing mode for the selected project.
  /// </summary>
  public bool IsEditing
  {
    get => _isEditing;
    set
    {
      _isEditing = value;
      NotifyPropertyChanged(nameof(IsEditing));
    }
  }
  private bool _isEditing;

  /// <summary>
  /// Update the project in the database.
  /// </summary>
  /// <param name="project"></param>
  public void UpdateProject(ProjectVM project)
  {
    // Save changes to the database
    var dbProject = dbContext.Projects.Find(project.ID);
    if (dbProject != null)
    {
      dbProject.Title = project.Title;
      dbContext.SaveChanges();
    }
  }
}
