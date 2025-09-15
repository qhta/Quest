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
    SpreadsheetControl.Open(fileName);
  }
}