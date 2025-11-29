using Path = System.IO.Path;

namespace QuestWPF;

/// <summary>
/// Command to open an Excel workbook to import data.
/// </summary>
public class FileImportCommand : Command
{

  /// <summary>
  /// A method to execute the command. If the parameter is null, it opens a file dialog to select a file.
  /// If the parameter is a string, it treats it as a file path.
  /// </summary>
  /// <param name="parameter"></param>
  public override async void Execute(object? parameter)
  {
    try
    {
      string? filename = parameter as string;
      if (filename == null)
      {
        // Open a file dialog to select a file
        var fileTypes = new[] { Strings.ExcelFilesFilter };
        //var ext = Path.GetExtension(filename)?.ToLowerInvariant();
        int filterIndex = 1;
        var openFileDialog = new OpenFileDialog
        {
          Title = Strings.OpenFileTitle,
          Filter = String.Join("|", fileTypes),
          FilterIndex = filterIndex,
        };

        if (openFileDialog.ShowDialog() == true)
          filename = openFileDialog.FileName;
        else
          return;
      }
      if (!String.IsNullOrEmpty(filename))
      {
        await OpenWorkbook(filename);
      }

    }
    catch (Exception e)
    {
      MessageBox.Show(e.Message);
    }
  }

  /// <summary>
  /// Opens an Excel workbook given its filename.
  /// First, it creates a WorkbookInfoVM instance and subscribes to its PropertyChanged event to monitor loading status.
  /// Before executing, it instructs CommandCenter to execute "AddFloatingView" command to add a floating view for the ExcelView.
  /// After that, it calls OpenSpreadsheetAsync on the ExcelView to load the selected spreadsheet asynchronously.
  /// When the WorkbookInfoVM's IsLoaded property changes, it notifies CommandCenter to update the CanExecute status of "StartImportCommand".  /// </summary>
  /// <param name="filename"></param>
  /// <returns></returns>
  public async Task<WorkbookInfoVM> OpenWorkbook(string filename)
  {
    var workbookInfoVM = new WorkbookInfoVM();
    workbookInfoVM.PropertyChanged += WorkbookInfoVM_PropertyChanged;
    var excelView = new ExcelView { FileName = filename, DataContext = workbookInfoVM };
    CommandCenter.ExecuteCommand(WindowCommands.OpenWindow, new WindowOpenData(excelView, "Workbook #", filename));

    await excelView.OpenSpreadsheetAsync(filename, workbookInfoVM);
    return workbookInfoVM;
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
