using Syncfusion.UI.Xaml.CellGrid;
using Syncfusion.UI.Xaml.Grid.ScrollAxis;
using Syncfusion.UI.Xaml.Spreadsheet;

using SelectionChangedEventArgs = System.Windows.Controls.SelectionChangedEventArgs;

namespace Quest.Views;

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
      excelView.UpdateSpreadsheetFileName(newFileName);
    }
  }

  // Method to update the SpreadsheetControl's filename
  private void UpdateSpreadsheetFileName(string fileName)
  {
    // Update filename property and load the new file
    SpreadsheetControl.FileName = fileName;

    IWorkbook workbook = SpreadsheetControl.Workbook;
    var xmlImporter = new XlsImporter();
    xmlImporter.OpenWorkbook(fileName);
    SpreadsheetControl.Open(xmlImporter.Workbook);
    var worksheets = new WorksheetInfoCollection(xmlImporter.GetWorksheets());
    WorksheetListView.ItemsSource = worksheets;

  }

  private void WorksheetListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    if (sender is ListView listView)
    {
      //IWorksheet

      if (listView.SelectedItem is WorksheetInfoVM worksheetInfo)
      {
        SpreadsheetControl.SetActiveSheet(worksheetInfo.Name);
        if (worksheetInfo.QuestRange != null)
        {
          var range = SpreadsheetControl.ActiveSheet.Range[worksheetInfo.QuestRange];
          if (range != null)
          {
            var parts = range.AddressR1C1Local.Split(':');
            var start = parts[0]; // e.g. R1C1
            var end = parts.Length > 1 ? parts[1] : parts[0]; // e.g. R20C4 or R1C1 if single cell
            int startRow = int.Parse(start.Substring(1, start.IndexOf('C') - 1));
            int startCol = int.Parse(start.Substring(start.IndexOf('C') + 1));
            int endRow = int.Parse(end.Substring(1, end.IndexOf('C') - 1));
            int endCol = int.Parse(end.Substring(end.IndexOf('C') + 1));
            SpreadsheetControl.ActiveGrid.SelectionController.ClearSelection();
            SpreadsheetControl.ActiveGrid.SelectionController.AddSelection(GridRangeInfo.Cells(startRow, startCol, endRow, endCol));
            SpreadsheetControl.ActiveGrid.ScrollInView(new RowColumnIndex(startRow - 1, startCol - 1));
            //SpreadsheetControl.InvalidateVisual();
            //SpreadsheetControl.UpdateLayout();
          }
        }
      }
    }
  }
}