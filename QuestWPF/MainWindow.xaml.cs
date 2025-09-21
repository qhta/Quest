using QuestRSX;
using QuestWPF.Views;

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
    OpenSpreadsheetCommand = new RelayCommand<Object>(OpenSpreadsheet);
    StartImportCommand = new RelayCommand<Object>(StartImport);
    InitializeComponent();
  }

  #region Open Spreadsheet Command
  /// <summary>
  /// Command to open an Excel spreadsheet file.
  /// </summary>
  public ICommand OpenSpreadsheetCommand { get; }

  private void OpenSpreadsheet(object? parameter)
  {
    // Open a file dialog to select an Excel file
    var openFileDialog = new OpenFileDialog
    {
      Filter = Strings.ExcelFilesFilter,
      Title = Strings.OpenExcelFileTitle
    };

    if (openFileDialog.ShowDialog() == true)
    {
      string newFilename = openFileDialog.FileName;
      var excelView = new ExcelView {  FileName = newFilename };
      excelView.OpenSpreadsheetAsync(newFilename);
      AddFloatingView(excelView, newFilename);
    }
  }
  #endregion

  #region Start Import Command
  /// <summary>
  /// Command to open an Excel spreadsheet file.
  /// </summary>
  public ICommand StartImportCommand { get; }

  private void StartImport(object? parameter)
  {
    if (parameter is not ExcelView excelView || excelView.DataContext is not WorkbookInfoVM workbookInfoVM)
    {
      MessageBox.Show("No workbook to import from.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      return;
    }

    // Open a file dialog to select an Excel file
    var saveFileDialog = new SaveFileDialog
    {
      FileName = System.IO.Path.GetFileName(System.IO.Path.ChangeExtension(excelView.FileName, ".db")),
      Filter = Strings.DatabaseFilesTitle,
      Title = Strings.CreateDatabaseFileTitle
    };

    if (saveFileDialog.ShowDialog() == true)
    {
      string newFilename = saveFileDialog.FileName;
      var questView = new QuestView { FileName = newFilename };
      questView.ImportSpreadsheetAsync(excelView.FileName, workbookInfoVM.Model, newFilename);
      AddFloatingView(questView, newFilename);
    }
  }
  #endregion

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