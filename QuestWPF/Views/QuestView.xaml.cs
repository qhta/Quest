using Quest.Data.QDM;

using QuestWPF.Helpers;

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
    var projectQualityVM = new ProjectQualityVM(new ProjectQuality());
    projectQualityVM.EvaluateValue();
    DataContext = projectQualityVM;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="QuestView"/> class.
  /// </summary>
  public QuestView(ProjectQualityVM projectQualityVM)
  {
    SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1JFaF5cXGpCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdmWXZfdXRQQmlYWUB+WERWYEg=");
    InitializeComponent();
    FileName = projectQualityVM.FileName;
    projectQualityVM.EvaluateValue();
    DataContext = projectQualityVM;
    projectQualityVM.IsExpanded = true;
    ExpandTreeViewItem(projectQualityVM);
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
    new PropertyMetadata(null));

  /// <summary>
  /// Name of the Quest file to be displayed.
  /// </summary>
  public string? ProjectName
  {
    get => (string?)GetValue(ProjectNameProperty);
    set => SetValue(ProjectNameProperty, value);
  }
  #endregion

  /// <summary>
  /// Opens the specified Excel file and asynchronously updates the QualityView and DataContext accordingly.
  /// </summary>
  /// <param name="excelFileName">Full path to excel file to import</param>
  /// <param name="workbookInfo">Info about Excel file to import. Contains recognized ranges of questionnaires and aggregation weights</param>
  public async Task<ProjectQuality> ImportExcelFileAsync(string excelFileName, WorkbookInfo workbookInfo)
  {
    var projectQualityVM = new ProjectQualityVM(new ProjectQuality());
    DataContext = projectQualityVM;
    projectQualityVM.IsLoading = true;
    projectQualityVM.TotalCount = workbookInfo.Worksheets.Count(item => item.IsSelected);
    using (var importer = new WorkbookImporter())
    {

      var workbook = importer.OpenWorkbook(excelFileName);
      projectQualityVM.ProjectTitle = workbookInfo.ProjectTitle;

      await importer.ImportProjectQualityAsync(workbook, workbookInfo, projectQualityVM.Model, Callback);

      projectQualityVM.ProjectTitle ??= QuestRSX.Strings.EmptyProjectTitle;
      projectQualityVM.IsLoading = false;
      projectQualityVM.IsLoaded = true;
      projectQualityVM.IsExpanded = true;
      ExpandTreeViewItem(projectQualityVM);
      return projectQualityVM.Model;
    }

    void Callback(IAsyncResult ar)
    {
      if (ar.AsyncState is QualityScale qualityScale)
        projectQualityVM.Scale = new QualityScaleVM(projectQualityVM, qualityScale);
      else if (ar.AsyncState is DocumentQuality documentQuality)
      {
        projectQualityVM.DocumentQualities ??= new DocumentQualityVMCollection(projectQualityVM, []);
        projectQualityVM.DocumentQualities.Add(new DocumentQualityVM(projectQualityVM, documentQuality));
        projectQualityVM.LoadedCount++;
      }
    }
  }

  private void ExpandTreeViewItem(ProjectQualityVM projectQualityVM)
  {
    if (ModelTreeView.ItemContainerGenerator.ContainerFromItem(projectQualityVM) is TreeViewItem container)
    {
      container.IsExpanded = projectQualityVM.IsExpanded;
    }
    else
    {
      ModelTreeView.ItemContainerGenerator.StatusChanged += (s, e) =>
      {
        if (ModelTreeView.ItemContainerGenerator.Status == System.Windows.Controls.Primitives.GeneratorStatus.ContainersGenerated)
        {
          if (ModelTreeView.ItemContainerGenerator.ContainerFromItem(projectQualityVM) is TreeViewItem generatedContainer)
          {
            generatedContainer.IsExpanded = projectQualityVM.IsExpanded;
          }
        }
      };
    }
  }
}