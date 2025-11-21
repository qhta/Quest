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
    CommandCenter.RegisterCommand(WindowsCommands.OpenWindow, AddFloatingViewCommand = new RelayCommand<WindowOpenData>(AddFloatingViewExecute));
    CommandCenter.RegisterCommand(ApplicationCommands.Open, OpenSpreadsheetCommand);
    CommandCenter.RegisterCommand(QuestCommands.Import, StartImportCommand);
    CommandCenter.RegisterCommand(FileCommands.Save, FileSaveCommand = FileSaveCommand, FileSaveData = new FileSaveData (MainWindow = mainWindow));
    ClearQuestionnaireCommand = new RelayCommand<Object>(ClearQuestionnaire, CanClearQuestionnaire);
  }


  /// <summary>
  /// Command to add a floating view to the MainWindow.
  /// </summary>
  public RelayCommand<WindowOpenData> AddFloatingViewCommand { get; }

  /// <summary>
  /// Command to open a spreadsheet. If a parameter is null, the user will be prompted to select a file.
  /// </summary>
  public OpenSpreadsheetCommand OpenSpreadsheetCommand { get; } = new OpenSpreadsheetCommand();

  /// <summary>
  /// Command to start the import process from the loaded spreadsheet.
  /// A parameter is expected to be an ExcelView instance with a loaded WorkbookInfoVM as its DataContext.
  /// </summary>
  public StartImportCommand StartImportCommand { get; } = new StartImportCommand();

  /// <summary>
  /// Command to save a file.
  /// A parameter should be of <see cref="IFileSaveData"/> type.
  /// </summary>
  public FileSaveCommand FileSaveCommand { get; } = new FileSaveCommand();

  /// <summary>
  /// Parameter for the <see cref="FileSaveCommand"/>.
  /// </summary>
  public FileSaveData FileSaveData { get; set; }

  //private void OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
  //{
  //  Debug.WriteLine($"MainWindow_OnCanExecute({(e.Command as RoutedUICommand)?.Text ?? e.Command.ToString()})");
  //  _Commander.OnCanExecute(sender, e);
  //  Debug.WriteLine($"MainWindow_OnCanExecute({(e.Command as RoutedUICommand)?.Text ?? e.Command.ToString()})={e.CanExecute}");
  //}

  //private void OnExecuted(object sender, ExecutedRoutedEventArgs e)
  //{
  //  // Delegate to the active UserControl
  //  if (TabControl.SelectedContent is IRoutedCommandHandler handler)
  //  {
  //    handler.OnExecuted(sender, e);
  //  }
  //}

  ///// <summary>
  ///// Handles the <see cref="INotifyPropertyChanged.PropertyChanged"/> event for the <see cref="WorkbookInfoVM"/>
  ///// instance.
  ///// </summary>
  ///// <remarks>This method listens for changes to the <see cref="WorkbookInfoVM.IsLoaded"/> property and
  ///// triggers a notification to update the execution status of the <see cref="StartImportCommand"/>.</remarks>
  ///// <param name="sender">The source of the event, typically the <see cref="WorkbookInfoVM"/> instance.</param>
  ///// <param name="e">The event data containing the name of the property that changed.</param>
  //public void WorkbookInfoVM_PropertyChanged(object? sender, PropertyChangedEventArgs e)
  //{
  //  if (e.PropertyName == nameof(WorkbookInfoVM.IsLoaded))
  //  {
  //    // Notify that CanExecute status has changed
  //    (StartImportCommand as RelayCommand<object>)?.NotifyCanExecuteChanged();
  //  }
  //}

  //#region OpenSpreadsheet Command
  ///// <summary>
  ///// Command to open an Excel spreadsheet file.
  ///// </summary>
  //public ICommand OpenSpreadsheetCommand { get; }

  //private async void OpenSpreadsheet(object? parameter)
  //{
  //  try
  //  {
  //    // Open a file dialog to select an Excel file
  //    var openFileDialog = new OpenFileDialog
  //    {
  //      Filter = Strings.ExcelFilesFilter,
  //      Title = Strings.OpenExcelFileTitle
  //    };

  //    if (openFileDialog.ShowDialog() == true)
  //    {
  //      string newFilename = openFileDialog.FileName;
  //      var workbookInfoVM = new WorkbookInfoVM();
  //      workbookInfoVM.PropertyChanged += WorkbookInfoVM_PropertyChanged;
  //      var excelView = new ExcelView { FileName = newFilename, DataContext = workbookInfoVM};
  //      AddFloatingView(excelView, newFilename);
  //      await excelView.OpenSpreadsheetAsync(newFilename, workbookInfoVM);
  //    }
  //  }
  //  catch (Exception e)
  //  {
  //    MessageBox.Show(e.Message);
  //  }
  //}
  //#endregion

  //#region StartImport Command
  ///// <summary>
  ///// Command to open an Excel spreadsheet file.
  ///// </summary>
  //public ICommand StartImportCommand { get; }

  //private bool CanStartImport(object? parameter)
  //{
  //  if (parameter is not ExcelView excelView || excelView.DataContext is not WorkbookInfoVM workbookInfoVM)
  //  {
  //    return false;
  //  }
  //  return workbookInfoVM.IsLoaded && workbookInfoVM.Model.Worksheets.Any(item => item.IsSelected);
  //}

  //private async void StartImport(object? parameter)
  //{
  //  try
  //  {
  //    if (parameter is not ExcelView excelView || excelView.DataContext is not WorkbookInfoVM workbookInfoVM)
  //    {
  //      MessageBox.Show("No workbook to import from.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
  //      return;
  //    }
  //    // Open a file dialog to select an Excel file
  //    var saveFileDialog = new SaveFileDialog
  //    {
  //      FileName = System.IO.Path.GetFileName(System.IO.Path.ChangeExtension(excelView.FileName, ".qxml")),
  //      Filter = Strings.QuestFilesTitle,
  //      Title = Strings.CreateQuestFileTitle
  //    };

  //    if (saveFileDialog.ShowDialog() == true)
  //    {
  //      string newFilename = saveFileDialog.FileName;
  //      var questView = new QuestView { FileName = newFilename };
  //      AddFloatingView(questView, newFilename);
  //      var projectQuality = await questView.ImportExcelFileAsync(excelView.FileName, workbookInfoVM.Model);
  //      new SharpSerializer().Serialize(projectQuality, newFilename);
  //      newFilename = System.IO.Path.ChangeExtension(newFilename, ".xml");
  //      await using (var writer = new StreamWriter(newFilename))
  //      {
  //        var xmlSerializer = new Qhta.Xml.Serialization.QXmlSerializer(typeof(ProjectQuality));
  //        xmlSerializer.Serialize(writer, projectQuality);
  //      }

  //      //var qdmContext = new QuestQDMDbContext();


  //    }
  //  }
  //  catch (Exception e)
  //  {
  //    MessageBox.Show(e.Message);
  //  }
  //}
  //#endregion

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