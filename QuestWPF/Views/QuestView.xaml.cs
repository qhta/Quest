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
    DataContext = new ProjectQualityVM(new ProjectQuality());
  }

  #region FileName Dependency Property
  /// <summary>
  /// DependencyProperty for the <see cref="FileName"/> property.
  /// </summary>
  public static readonly DependencyProperty FileNameProperty = DependencyProperty.Register
  (nameof(FileName), typeof(string), typeof(QuestView),
    new PropertyMetadata(null, OnFileNameChanged));

  /// <summary>
  /// Name of the Quest file to be displayed.
  /// </summary>
  public string? FileName
  {
    get => (string?)GetValue(FileNameProperty);
    set => SetValue(FileNameProperty, value);
  }

  // Callback method for when the FileName property changes
  private static void OnFileNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
  }
  #endregion

  #region ProjectName Dependency Property
  /// <summary>
  /// DependencyProperty for the <see cref="ProjectName"/> property.
  /// </summary>
  public static readonly DependencyProperty ProjectNameProperty = DependencyProperty.Register
  (nameof(ProjectName), typeof(string), typeof(QuestView),
    new PropertyMetadata(null, OnProjectNameChanged));

  /// <summary>
  /// Name of the Quest file to be displayed.
  /// </summary>
  public string? ProjectName
  {
    get => (string?)GetValue(ProjectNameProperty);
    set => SetValue(ProjectNameProperty, value);
  }

  // Callback method for when the ProjectName property changes
  private static void OnProjectNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (d is QuestView questView)
    {
      if (questView.DataContext is ProjectQualityVM vm)
      {
        vm.ProjectName = e.NewValue as string;
      }
    }
  }
  #endregion
}