namespace QuestWPF;

/// <summary>
/// Main commander responsible for managing operations within the system.
/// </summary>
public class _Commander
{
  /// <summary>
  /// Required reference to the MainWindow instance.
  /// </summary>
  public MainWindow MainWindow { get; set; }

  /// <summary>
  /// Default constructor creating the commander and registering commands.
  /// </summary>
  public _Commander(MainWindow mainWindow)
  {
    MainWindow = mainWindow;
    CommandCenter.RegisterCommand(FileCommands.Import, FileImportCommand = new FileImportCommand());
    CommandCenter.RegisterCommand(FileCommands.Open, FileOpenCommand = new FileOpenCommand());
    CommandCenter.RegisterCommand(WindowCommands.OpenWindow, AddFloatingViewCommand = new RelayCommand<WindowOpenData>(AddFloatingViewExecute));
    CommandCenter.RegisterCommand(QuestCommands.Import, StartImportCommand = new StartImportCommand());
    CommandCenter.RegisterCommand(FileCommands.Save, FileSaveCommand, FileSaveData = new FileSaveData(mainWindow));
    CommandCenter.RegisterCommand(FileCommands.SaveAs, FileSaveCommand, FileSaveAsData = new FileSaveData(mainWindow, true));
    CommandCenter.RegisterCommand(ApplicationCommands.Copy, ViewCopyCommand, ViewCopyData = new ViewCopyData(mainWindow));
    ClearQuestionnaireCommand = new RelayCommand<Object>(ClearQuestionnaire, CanClearQuestionnaire);
  }


  /// <summary>
  /// Command to add a floating view to the MainWindow.
  /// </summary>
  public RelayCommand<WindowOpenData> AddFloatingViewCommand { get; }

  /// <summary>
  /// Command to open a spreadsheet. If a parameter is null, the user will be prompted to select a file.
  /// </summary>
  public FileImportCommand FileImportCommand { get; }

  /// <summary>
  /// Command to open a spreadsheet. If a parameter is null, the user will be prompted to select a file.
  /// </summary>
  public FileOpenCommand FileOpenCommand { get; }

  /// <summary>
  /// Command to start the import process from the loaded spreadsheet.
  /// A parameter is expected to be an ExcelView instance with a loaded WorkbookInfoVM as its DataContext.
  /// </summary>
  public StartImportCommand StartImportCommand { get; }

  /// <summary>
  /// Command to save a file.
  /// A parameter should be of <see cref="FileSaveData"/> type.
  /// </summary>
  public FileSaveCommand FileSaveCommand { get; } = new();

  /// <summary>
  /// Parameter for the <see cref="FileSaveCommand"/>.
  /// </summary>
  public FileSaveData FileSaveData { get; }

  /// <summary>
  /// Parameter for the <see cref="FileSaveCommand"/> registered as <see cref="FileCommands.SaveAs"/>.
  /// </summary>
  public FileSaveData FileSaveAsData { get; }

  /// <summary>
  /// Command to copy current view to clipboard.
  /// </summary>
  public ViewCopyCommand ViewCopyCommand { get; } = new();

  /// <summary>
  /// Parameter for the <see cref="ViewCopyCommand"/>.
  /// </summary>
  public ViewCopyData ViewCopyData { get; }

  #region ClearQuestionnaire Command
  /// <summary>
  /// Command to open an Excel spreadsheet file.
  /// </summary>
  public ICommand ClearQuestionnaireCommand { get; }

  private bool CanClearQuestionnaire(object? parameter)
  {
    if (parameter is not DocumentQualityVM && parameter is not ProjectQualityVM)
    {
      return false;
    }
    return true;
  }

  private void ClearQuestionnaire(object? parameter)
  {
    try
    {
      if (parameter is DocumentQualityVM documentQualityVM)
      {
        ClearQuestionnaire(documentQualityVM);
      }
      else
      if (parameter is ProjectQualityVM projectQualityVM)
      {
        if (projectQualityVM.DocumentQualities != null)
          foreach (var item in projectQualityVM.DocumentQualities)
            ClearQuestionnaire(item);
      }
    }
    catch (Exception e)
    {
      MessageBox.Show(e.Message);
    }
  }

  private void ClearQuestionnaire(DocumentQualityVM documentQualityVM)
  {
    foreach (var item in documentQualityVM.Factors)
    {
      ClearQuestionnaire(item);
    }
  }

  private void ClearQuestionnaire(IQualityNodeVM qualityNode)
  {
    if (qualityNode is QualityMeasureVM qualityMeasure)
    {
      qualityMeasure.Grade = null;
      qualityMeasure.Value = null;
      qualityMeasure.Comment = null;
    }
    else
    {
      qualityNode.Value = null;
      qualityNode.Comment = null;
    }
  }
  #endregion

  /// <summary>
  /// Add a view to the docking manager.
  /// </summary>
  public void AddFloatingViewExecute(WindowOpenData? parameters)
  {
    if (parameters != null && parameters?.Content is Control control)
    {
      MainWindow.AddFloatingView(control, parameters.Name, parameters.Title);
    }
    else throw new InvalidOperationException($"Parameters of the AddFloatingView are invalid");
  }

}