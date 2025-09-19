namespace QuestWPF.Views;

/// <summary>
/// View for displaying and interacting with questionnaires.
/// </summary>
public partial class QuestView : UserControl
{
  /// <summary>
  /// Initializes a new instance of the <see cref="QuestView"/> class.
  /// </summary>
  public QuestView()
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
  /// Name of the Quest file to be displayed.
  /// </summary>
  public string FileName
  {
    get => (string)GetValue(FileNameProperty);
    set => SetValue(FileNameProperty, value);
  }

  // Callback method for when the FileName property changes
  private static void OnFileNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
  }
}