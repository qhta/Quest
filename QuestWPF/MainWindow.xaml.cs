using Quest.Views;

namespace QuestWPF;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
  /// <summary>
  /// Initializes a new instance of the <see cref="MainWindow"/> class.
  /// </summary>
  /// <remarks>This constructor registers the Syncfusion license key and initializes the components of the
  /// main window. Ensure that the Syncfusion license key is valid and properly configured before using this
  /// application.</remarks>
  public MainWindow()
  {
    // Register the Syncfusion license key
    SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1JFaF5cXGpCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdmWXZfdXRQQmlYWUB+WERWYEg=");
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

      // Pass the file path to the ExcelView or handle it as needed
      MessageBox.Show($"Selected file: {filePath}", "Open Spreadsheet");
      var excelView = new ExcelView { FileName = filePath };
      var tabItem = new TabItem{ Header = filePath, Content = excelView};
      MainTabControl.Items.Add(tabItem);
      MainTabControl.SelectedItem = tabItem;
    }
  }
}