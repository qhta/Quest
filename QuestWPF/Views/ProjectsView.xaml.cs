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

    //// Load projects into the collection
    viewModel.LoadProjects();

    // Bind the Projects property to a UI control
    DataContext = viewModel;
  }

  private void ProjectsListView_KeyDown(object sender, KeyEventArgs e)
  {
    if (e.Key == Key.F2 && ProjectsListView.SelectedItem != null)
    {
      // Enable editing mode
      if (DataContext is ProjectsViewModel viewModel)
      {
        viewModel.IsEditing = true;
      }

      // Focus the TextBox
      EditTextBox.Focus();
      EditTextBox.SelectAll();
    }
  }

  private void EditTextBox_KeyDown(object sender, KeyEventArgs e)
  {
    if (e.Key == Key.Enter)
    {
      // Exit editing mode
      var viewModel = ProjectsViewModel.Instance;
      viewModel.IsEditing = false;
      // Update the database or perform any additional actions
      var selectedProject = viewModel.SelectedProject;
      if (selectedProject != null)
      {
        // Save changes to the database
        viewModel.UpdateProject(selectedProject);
      }
    }
    else
    if (e.Key == Key.Escape)
    {
      // Exit editing mode
      var viewModel = ProjectsViewModel.Instance;

      if (viewModel.IsEditing && viewModel.SelectedProject != null)
      {
        // Reload the original title from the database
        var dbContext = new QuestRDM.QuestRdmDbContext();
        var dbProject = dbContext.Projects.Find(viewModel.SelectedProject.ID);
        if (dbProject != null)
        {
          viewModel.SelectedProject.Title = dbProject.Title;
        }
        viewModel.IsEditing = false;
      }
    }

  }
}
