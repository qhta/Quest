using System.Collections.ObjectModel;

using Quest.Views;

using Syncfusion.Windows.Tools.Controls;

namespace QuestWPF;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
  /// <summary>
  /// Initializes a new instance of the <see cref="MainWindow"/> class.
  /// </summary>
  /// <remarks>This constructor initializes the components of the
  /// main window. Ensure that the Syncfusion license key is valid and properly configured before using this
  /// application.</remarks>
  public MainWindow()
  {

    // Initialize the OpenSpreadsheet command
    OpenSpreadsheetCommand = new RelayCommand<Object>(OpenSpreadsheet);
    InitializeComponent();
  }

  /// <summary>
  /// Command to open and Excel spreadsheet file.
  /// </summary>
  public ICommand OpenSpreadsheetCommand { get; }

  private void OpenSpreadsheet(object? parameter)
  {
    // Open a file dialog to select an Excel file
    OpenFileDialog openFileDialog = new OpenFileDialog
    {
      Filter = "Excel Files|*.xlsx;*.xlsm;*.xls",
      Title = "Open Spreadsheet"
    };

    if (openFileDialog.ShowDialog() == true)
    {
      string filePath = openFileDialog.FileName;
      var excelView = new ExcelView { FileName = filePath };
      AddFloatingView(excelView, filePath);
    }
  }

  /// <summary>
  /// Add a view to the docking manager.
  /// </summary>
  /// <param name="view"></param>
  /// <param name="header"></param>
  public void AddFloatingView(Control view, string? header)
  {
    if (DataContext is MDIViewModel viewModel)
    {
      var dockItem = new DockItem
      {
        Header = header,
        State = DockState.Float,
        CanFloatMaximize = true,
        Content = view
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
      double width = 800;
      double height = 600;

      // Final rect in screen coordinates (DIPs)
      dockItem.FloatingWindowRect = new Rect(
        screenTopLeftPx.X + offset,
        screenTopLeftPx.Y + offset,
        width,
        height);
    }
  }
}