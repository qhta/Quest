using QuestIMP;

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
      new PropertyMetadata(null, OnFileNameChanged));

  /// <summary>
  /// Name of the Excel file to be displayed.
  /// </summary>
  public string FileName
  {
    get => (string)GetValue(FileNameProperty);
    set => SetValue(FileNameProperty, value);
  }

  // Callback method for when the FileName property changes
  private static void OnFileNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (d is ExcelView excelView && e.NewValue is string newFileName)
    {
      // Assuming SpreadsheetControl is a part of ExcelView
      excelView.UpdateSpreadsheetFileNameAsync(newFileName);
    }
  }

  private void UpdateSpreadsheetFileName(string fileName)
  {
    try
    {
      //SpreadsheetControl.FileName = fileName;
      SpreadsheetControl.Open(fileName);
      var workbookInfo = GetWorkbookInfo(fileName);
      var workbookVM = new WorkbookInfoVM(workbookInfo);
      workbookVM.ProjectTitle ??= "<Tytuł projektu>";
      DataContext = workbookVM;
    }
    catch (Exception e)
    {
      Debug.WriteLine(e);
    }
  }

  // Method to update the SpreadsheetControl's filename
  private async void UpdateSpreadsheetFileNameAsync(string fileName)
  {
    try
    {
      //SpreadsheetControl.FileName = fileName;
      SpreadsheetControl.Open(fileName);
      var workbookInfo = await GetWorkbookInfoAsync(fileName);
      var workbookVM = new WorkbookInfoVM(workbookInfo);
      workbookVM.ProjectTitle ??= "<Tytuł projektu>";
      DataContext = workbookVM;
    }
    catch (Exception e)
    {
      Debug.WriteLine(e);
    }
  }

  private WorkbookInfo GetWorkbookInfo(string fileName)
  {
      var xmlImporter = new XlsImporter();
      xmlImporter.OpenWorkbook(fileName);
      var workbookInfo = xmlImporter.GetWorkbookInfo(fileName);
      return workbookInfo;
  }

  private async Task<WorkbookInfo> GetWorkbookInfoAsync(string fileName)
  {
    return await Task.Run(() =>
    {
      var xmlImporter = new XlsImporter();
      xmlImporter.OpenWorkbook(fileName);
      var workbookInfo = xmlImporter.GetWorkbookInfo(fileName);
      return workbookInfo;
    });
  }

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