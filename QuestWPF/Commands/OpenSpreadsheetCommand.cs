namespace QuestWPF;

/// <summary>
/// Command to open an Excel spreadsheet file.
/// </summary>
public class OpenSpreadsheetCommand : Command
{

  /// <summary>
  /// A method to execute the command. If the parameter is null, it opens a file dialog to select an Excel file.
  /// First, it creates a WorkbookInfoVM instance and subscribes to its PropertyChanged event to monitor loading status.
  /// Before executing, it instructs CommandCenter to execute "AddFloatingView" command to add a floating view for the ExcelView.
  /// After that, it calls OpenSpreadsheetAsync on the ExcelView to load the selected spreadsheet asynchronously.
  /// When the WorkbookInfoVM's IsLoaded property changes, it notifies CommandCenter to update the CanExecute status of "StartImportCommand".
  /// </summary>
  /// <param name="parameter"></param>
  public override async void Execute(object? parameter)
  {
    try
    {
      string? filename = parameter as string;
      if (filename == null)
      {
        // Open a file dialog to select an Excel file
        var openFileDialog = new OpenFileDialog
        {
          Filter = Strings.ExcelFilesFilter,
          Title = Strings.OpenExcelFileTitle
        };

        if (openFileDialog.ShowDialog() == true)
          filename = openFileDialog.FileName;
      }
      if (!String.IsNullOrEmpty(filename))
      {
        var workbookInfoVM = new WorkbookInfoVM();
        workbookInfoVM.PropertyChanged += WorkbookInfoVM_PropertyChanged;
        var excelView = new ExcelView { FileName = filename, DataContext = workbookInfoVM };
        CommandCenter.ExecuteCommand(WindowsCommands.OpenWindow, new WindowOpenData(excelView, "Workbook #", filename));

        await excelView.OpenSpreadsheetAsync(filename, workbookInfoVM);
      }
    }
    catch (Exception e)
    {
      MessageBox.Show(e.Message);
    }
  }

  private void WorkbookInfoVM_PropertyChanged(object? sender, PropertyChangedEventArgs e)
  {
    if (e.PropertyName == nameof(WorkbookInfoVM.IsLoaded))
    {
      // Notify that CanExecute status has changed
      CommandCenter.NotifyCanExecuteChanged(QuestCommands.Import);
      CommandCenter.NotifyCanExecuteChanged(FileCommands.Save);

    }
  }
}