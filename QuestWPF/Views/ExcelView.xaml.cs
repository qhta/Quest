namespace QuestWPF.Views;

/// <summary>
/// Interaction logic for ExcelView.xaml
/// </summary>
public partial class ExcelView : UserControl
{
  /// <summary>
  /// Initializes a new instance of the <see cref="ExcelView"/> class.
  /// </summary>
  /// <remarks>This constructor sets up the necessary components for the <see cref="ExcelView"/> instance.
  /// Ensure that the required dependencies are properly configured before using this class.</remarks>
  public ExcelView()
  {
    SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1JFaF5cXGpCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdmWXZfdXRQQmlYWUB+WERWYEg=");
    InitializeComponent();
  }

  /// <summary>
  /// DependencyProperty for the <see cref="FileName"/> property.
  /// </summary>
  public static readonly DependencyProperty FileNameProperty = DependencyProperty.Register
    (nameof(FileName), typeof(string), typeof(ExcelView),
      new PropertyMetadata(null));

  /// <summary>
  /// Name of the Excel file to be displayed.
  /// </summary>
  public string FileName
  {
    get => (string)GetValue(FileNameProperty);
    set => SetValue(FileNameProperty, value);
  }

  /// <summary>
  /// Opens the specified Excel file and updates the SpreadsheetControl and DataContext accordingly.
  /// </summary>
  /// <param name="fileName"></param>
  public void OpenSpreadsheet(string fileName)
  {
    try
    {
      FileName = fileName;
      SpreadsheetControl.Open(fileName);
      var workbookInfo = GetWorkbookInfo(fileName);
      var workbookVM = new WorkbookInfoVM(workbookInfo);
      workbookVM.ProjectTitle ??= QuestRSX.Strings.EmptyProjectTitle;
      DataContext = workbookVM;
    }
    catch (Exception e)
    {
      Debug.WriteLine(e);
    }
  }

  /// <summary>
  /// Opens the specified Excel file asynchronously and updates the SpreadsheetControl and DataContext accordingly.
  /// </summary>
  /// <param name="fileName"></param>
  public async void OpenSpreadsheetAsync(string fileName)
  {
    try
    {
      FileName = fileName;
      SpreadsheetControl.Open(fileName);
      var workbookVM = new WorkbookInfoVM{ FileName = fileName };
      workbookVM.IsLoading = true;
      DataContext = workbookVM;
      var xlsImporter = new XlsImporter();
      xlsImporter.OpenWorkbook(fileName);
      workbookVM.WorksheetsCount = xlsImporter.Workbook!.Worksheets.Count;
      await GetWorkbookInfoAsync(xlsImporter, workbookVM);
      workbookVM.ProjectTitle ??= QuestRSX.Strings.EmptyProjectTitle;
      workbookVM.IsLoading = false;
    }
    catch (Exception e)
    {
      Debug.WriteLine(e);
    }
  }

  /// <summary>
  /// Retrieves information about the specified workbook file.
  /// </summary>
  /// <param name="fileName">The full path to the workbook file to be analyzed. This cannot be null or empty.</param>
  /// <returns>A <see cref="WorkbookInfo"/> object containing metadata and details about the workbook.</returns>
  private WorkbookInfo GetWorkbookInfo(string fileName)
  {
      var xlsImporter = new XlsImporter();
      xlsImporter.OpenWorkbook(fileName);
      var workbookInfo = xlsImporter.GetWorkbookInfo(fileName);
      return workbookInfo;
  }

  /// <summary>
  /// Retrieves and populates the workbook information asynchronously.
  /// </summary>
  /// <param name="xlsImporter"></param>
  /// <param name="workbookVM"></param>
  /// <returns></returns>
  private async Task GetWorkbookInfoAsync(XlsImporter xlsImporter, WorkbookInfoVM workbookVM)
  {
    workbookVM.ProjectTitle = await xlsImporter.ScanForProjectTitleAsync();
    var worksheetInfos = xlsImporter.GetWorksheetsAsync();

    await foreach (var worksheetInfo in worksheetInfos)
    {
      //Debug.WriteLine($"Add {worksheetInfo.Name}");
      workbookVM.Worksheets.Add(new WorksheetInfoVM(worksheetInfo));
      workbookVM.LoadedCount++;
    }
  }

  /// <summary>
  /// Method to handle selection changes in the worksheet list view.
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  private void WorksheetListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    if (sender is ListView listView)
    {
      if (listView.SelectedItem is WorksheetInfoVM worksheetInfo)
      {
        //Debug.WriteLine($"SelectionChanged({worksheetInfo.Name})");
        SpreadsheetControl.SetActiveSheet(worksheetInfo.Name);
      }
    }
  }

  /// <summary>
  /// Method to handle mouse left button down event on the range text block.
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  private void RangeTextBlock_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    if (WorksheetListView.SelectedItem is WorksheetInfoVM worksheetInfo)
    {
      SpreadsheetControl.SetActiveSheet(worksheetInfo.Name);
      if (sender is TextBlock textBlock)
      {
        var text = textBlock.Text;
        //Debug.WriteLine($"RangeTextBlock_OnMouseLeftButton({worksheetInfo.Name},{text})");
        var range = SpreadsheetControl.ActiveSheet.Range[text];
        SelectRange(range);
      }
    }
  }

  /// <summary>
  /// Helper method to select a range in the active sheet based on the given IRange object.
  /// </summary>
  /// <param name="range"></param>
  private void SelectRange(IRange range)
  {
    var parts = range.AddressR1C1Local.Split(':');
    var start = parts[0]; // e.g. R1C1
    var end = parts.Length > 1 ? parts[1] : parts[0]; // e.g. R20C4 or R1C1 if single cell
    int startRow = int.Parse(start.Substring(1, start.IndexOf('C') - 1));
    int startCol = int.Parse(start.Substring(start.IndexOf('C') + 1));
    int endRow = int.Parse(end.Substring(1, end.IndexOf('C') - 1));
    int endCol = int.Parse(end.Substring(end.IndexOf('C') + 1));
    SpreadsheetControl.ActiveGrid.SelectionController.AddSelection(GridRangeInfo.Cells(startRow, startCol, endRow, endCol));
  }

}