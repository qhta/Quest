namespace Quest;

/// <summary>
/// Observable collection of <see cref="ProjectVM"/> objects.
/// </summary>
public class ProjectVMCollection : ObservableList<ProjectVM>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="ProjectVMCollection"/> class with a list of <see cref="Project"/>.
  /// </summary>
  /// <param name="items"></param>
  public ProjectVMCollection(IEnumerable<Project> items) : base(items.Select(item => new ProjectVM(item)))
  {
  }
}