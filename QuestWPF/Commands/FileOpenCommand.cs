using Quest;

using Path = System.IO.Path;

namespace QuestWPF;

/// <summary>
/// Command to open a Quest project file.
/// </summary>
public class FileOpenCommand : Command
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
        var fileTypes = new[] { Strings.QuestFilesFilter };
        //var ext = Path.GetExtension(filename)?.ToLowerInvariant();
        int filterIndex = 1;
        var openFileDialog = new OpenFileDialog
        {
          Title = Strings.OpenFile,
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
        await OpenProjectQuality(filename);
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
  public async Task<ProjectQualityVM?> OpenProjectQuality(string filename)
  {
    ProjectQuality? projectQuality = null;
    try
    {
      await Task.Run(() =>
      {
        using (var reader = new StreamReader(filename))
        {
          var xmlSerializer = new Qhta.Xml.Serialization.QXmlSerializer(typeof(ProjectQuality));
          projectQuality = xmlSerializer.Deserialize(reader) as ProjectQuality;
          //projectQualityVM.PropertyChanged += WorkbookInfoVM_PropertyChanged;
        }
      });
      if (projectQuality != null)
      {
        var projectQualityVM = new ProjectQualityVM(projectQuality);
        var questView = new QuestView(projectQualityVM);
        CommandCenter.ExecuteCommand(WindowCommands.OpenWindow, new WindowOpenData(questView, "Quest #", filename));
        return projectQualityVM;
      }

    } catch (Exception e)
    {
      MessageBox.Show(e.Message);
    }
    return null;
  }

}
