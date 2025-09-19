namespace QuestWPF.Views;
/// <summary>
/// Interaction logic for ProjectsView.xaml
/// </summary>
public partial class ProjectsView : UserControl
{
  private ScrollViewer? _listScrollViewer;

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
  private void ProjectsListView_Loaded(object sender, RoutedEventArgs e)
  {
    // Hook into the internal ScrollViewer to react when scrollbar appears/disappears
    _listScrollViewer = FindVisualChild<ScrollViewer>(ProjectsListView);
    if (_listScrollViewer is not null)
    {
      _listScrollViewer.ScrollChanged += (_, __) => ResizeListColumnToFill();
    }
    ResizeListColumnToFill();
  }

  private void ProjectsListView_SizeChanged(object sender, SizeChangedEventArgs e)
  {
    ResizeListColumnToFill();
  }

  private void ResizeListColumnToFill()
  {
    if (ProjectsListView.View is not GridView gv || gv.Columns.Count == 0) return;

    // Subtract vertical scrollbar width only when visible
    double vbar = (_listScrollViewer?.ComputedVerticalScrollBarVisibility == (Visibility.Visible))
      ? SystemParameters.VerticalScrollBarWidth
      : 0;

    // Small padding fudge to avoid horizontal scrollbar
    double padding = 8;

    double newWidth = Math.Max(0, ProjectsListView.ActualWidth - vbar - padding);
    if (!Double.IsNaN(newWidth) && !Double.IsInfinity(newWidth))
    {
      gv.Columns[0].Width = newWidth;
    }
  }

  private static TChild? FindVisualChild<TChild>(DependencyObject? parent) where TChild : DependencyObject
  {
    if (parent is null) return null;
    for (int i = 0, n = VisualTreeHelper.GetChildrenCount(parent); i < n; i++)
    {
      var child = VisualTreeHelper.GetChild(parent, i);
      if (child is TChild typed) return typed;
      var nested = FindVisualChild<TChild>(child);
      if (nested is not null) return nested;
    }
    return null;
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
