namespace Quest;

/// <summary>
/// Observable collection of <see cref="ProjectVM"/> objects.
/// </summary>
public class ProjectsCollection : ObservableList<ProjectVM>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="ProjectsCollection"/> class with a list of <see cref="Project"/>.
  /// </summary>
  /// <param name="items"></param>
  public ProjectsCollection(IEnumerable<Project> items) : base(items.Select(item => new ProjectVM(item)))
  {
  }
}