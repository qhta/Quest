
using QuestWPF.Views;
using Path = System.IO.Path;

namespace QuestWPF;

/// <summary>
/// Command to start the import process from the loaded spreadsheet.
/// A parameter is expected to be an ExcelView instance with a loaded WorkbookInfoVM as its DataContext.
/// </summary>
public class StartImportCommand : Command
{
  /// <inheritdoc/>
  public override bool CanExecute(object? parameter)
  {
    if (parameter is not ExcelView excelView || excelView.DataContext is not WorkbookInfoVM workbookInfoVM)
    {
      return false;
    }
    return workbookInfoVM.IsLoaded && workbookInfoVM.Model.Worksheets.Any(item => item.IsSelected);
  }

  /// <inheritdoc/>
  public override async void Execute(object? parameter)
  {
    try
    {
      if (parameter is not ExcelView excelView || excelView.DataContext is not WorkbookInfoVM workbookInfoVM)
      {
        MessageBox.Show("No workbook to import from.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        return;
      }
      string newFilename = Path.GetFileNameWithoutExtension(excelView.FileName);
      var questView = new QuestView { FileName = newFilename };
      CommandCenter.ExecuteCommand(WindowCommands.OpenWindow, new WindowOpenData(questView, "Quest #", newFilename));
      var projectQuality = await questView.ImportExcelFileAsync(excelView.FileName, workbookInfoVM.Model);
    }
    catch (Exception e)
    {
      MessageBox.Show(e.Message);
    }
  }

}