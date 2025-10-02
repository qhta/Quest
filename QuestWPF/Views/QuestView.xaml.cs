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
    DataContext = projectQualityVM;
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
  /// <param name="dbFileName">Full path to database file to create</param>
  public async void ImportSpreadsheetAsync(string excelFileName, WorkbookInfo workbookInfo, string dbFileName)
  {
    try
    {
      FileName = dbFileName;
      var qualityVM = new ProjectQualityVM(new ProjectQuality());
      DataContext = qualityVM;
      qualityVM.IsLoading = true;
      qualityVM.TotalCount = workbookInfo.Worksheets.Count(item => item.IsSelected);
      var workbook = WorkbookImporter.OpenWorkbook(excelFileName);

      await ImportWorkbookAsync(workbook, workbookInfo, qualityVM);
      qualityVM.ProjectTitle ??= QuestRSX.Strings.EmptyProjectTitle;
      qualityVM.IsLoading = false;
      qualityVM.IsLoaded = true;
      qualityVM.IsExpanded = true;

      ExpandTreeViewItem(qualityVM);
    }
    catch (Exception e)
    {
      Debug.WriteLine(e);
    }
  }

  /// <summary>
  /// Retrieves data from workbook and populates the quality view model asynchronously.
  /// </summary>
  /// <param name="workbook">Opened Excel workbook</param>
  /// <param name="workbookInfo">Workbook info object</param>
  /// <param name="projectQualityVM">Project quality view model</param>
  /// <returns></returns>
  private async Task ImportWorkbookAsync(IWorkbook workbook, WorkbookInfo workbookInfo, ProjectQualityVM projectQualityVM)
  {
    projectQualityVM.ProjectTitle = workbookInfo.ProjectTitle;
    var worksheetWithScale = workbookInfo.Worksheets.FirstOrDefault(item => item.ScaleRange != null);
    if (worksheetWithScale != null)
    {
      projectQualityVM.Model.Scale = WorkbookImporter.ImportScaleTable(workbook.Worksheets[worksheetWithScale.Name], worksheetWithScale);
      projectQualityVM.Scale = new QualityScaleVM(projectQualityVM, projectQualityVM.Model.Scale!);
    }

    var worksheetInfos = WorkbookImporter.ImportDocumentQualitiesAsync(workbook, workbookInfo);
    await foreach (var worksheetInfo in worksheetInfos)
    {
      //Debug.WriteLine($"Add {worksheetInfo.Name}");
      projectQualityVM.DocumentQualities.Add(new DocumentQualityVM(projectQualityVM, worksheetInfo));
      projectQualityVM.LoadedCount++;
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