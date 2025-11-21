namespace QuestWPF;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
  /// <summary>
  /// Initializes a new instance of the <see cref="MainWindow"/> class.
  /// </summary>
  /// <remarks>
  /// This constructor initializes the components of the main window.
  /// Ensure that the Syncfusion license key is valid and properly configured before using this application.
  /// </remarks>
  public MainWindow()
  {
    Commander = new _Commander ( this );
    InitializeComponent();
    //if (DataContext is MDIViewModel viewModel)
    //{
    //  viewModel.ActiveViewChangedOn(dockingManager);
    //  viewModel.ActiveViewChanged += ViewModel_ActiveViewChanged;
    //}
  }

  private void ViewModel_ActiveViewChanged(object sender, DependencyPropertyChangedEventArgs e)
  {
    ActiveView = (e.NewValue as DockItem)?.Content as Control;
  }

  /// <summary>
  /// Gets the commander responsible for managing operations within the system.
  /// </summary>
  public _Commander Commander { get; }

  /// <summary>
  /// Add a view to the docking manager.
  /// </summary>
  /// <param name="view"></param>
  /// <param name="windowName"></param>
  /// <param name="header"></param>
  public void AddFloatingView(Control view, string windowName, string? header)
  {
    if (DataContext is MDIViewModel viewModel)
    {
      if (windowName.Contains('#'))
      {
        var nameCount = viewModel.DockCollections.Count(dockItem => dockItem.Name.StartsWith(windowName))+1;
        windowName =windowName.Replace("#",nameCount.ToString()).Replace(" ","_");
      }

      var dockItem = new DockItem
      {
        Name =  windowName,
        Header = header,
        State = DockState.Float,
        CanFloatMaximize = true,
        Content = view,
      };

      viewModel.DockCollections.Add(dockItem);
      // Get top-left of dockingManager in screen pixels
      Point screenTopLeftPx = dockingManager.PointToScreen(new Point(0, 0));

      // Convert to device-independent units (DIPs) if DPI scaling is not 100%
      var source = PresentationSource.FromVisual(dockingManager);
      if (source?.CompositionTarget is not null)
      {
        // TransformFromDevice converts physical pixels -> DIPs
        var toDip = source.CompositionTarget.TransformFromDevice;
        screenTopLeftPx = toDip.Transform(screenTopLeftPx);
      }
      var n = viewModel.DockCollections.Count;
      // Relative cascade offset in DIPs
      double offset = 20 * n;

      // Desired size
      double width = 1210;
      double height = 830;

      // Final rect in screen coordinates (DIPs)
      dockItem.FloatingWindowRect = new Rect(
        screenTopLeftPx.X + offset,
        screenTopLeftPx.Y + offset,
        width,
        height);
      dockingManager.ActivateWindow(windowName);
    }
  }

  /// <summary>
  /// Gets or sets the currently active view in the MainWindow.
  /// </summary>
  public Control? ActiveView { get; set; }


  private void DockingManager_OnActiveWindowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    var activeWindow = e.NewValue;
    if (activeWindow is ContentControl contentControl)
    {
      ActiveView = contentControl.Content as Control;
      Commander.FileSaveCommand.NotifyCanExecuteChanged();
    }
  }
}