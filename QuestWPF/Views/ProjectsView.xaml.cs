namespace Quest.Views;
/// <summary>
/// Interaction logic for ProjectsView.xaml
/// </summary>
public partial class ProjectsView : UserControl
{
  /// <summary>
  /// Initializing constructor
  /// </summary>
  public ProjectsView()
  {
    InitializeComponent();
    // Access the singleton instance
    var viewModel = ProjectsViewModel.Instance;

    // Load projects into the collection
    viewModel.LoadProjects();

    // Bind the Projects property to a UI control
    DataContext = viewModel.Projects;
  }
}
