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
        var fileTypes = new[] { FilenameTools.MakeFilterString(Strings.QuestXmlFiles, ".xml"),
                                FilenameTools.MakeFilterString(Strings.QuestZipFiles, ".zip")};
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
        await OpenQuestFile(filename);
      }

    }
    catch (Exception e)
    {
      MessageBox.Show(e.Message);
    }
  }

  /// <summary>
  /// Opens a Quest file given its filename.
  /// It uses the appropriate method of FileCommandHelper to read the file contents.
  /// Then, it creates a ProjectQualityVM from the ProjectQuality object
  /// and then opens a new QuestView window to display the project.
  /// </summary>
  /// <param name="filename"></param>
  /// <returns></returns>
  public async Task<ProjectQualityVM?> OpenQuestFile(string filename)
  {
    ProjectQuality? projectQuality = null;
    try
    {
      projectQuality = filename.ToLower().EndsWith("xml") ?
        await FileCommandHelper.DeserializeProjectAsync(await File.ReadAllBytesAsync(filename)) :
        await FileCommandHelper.UnpackProjectAsync(await File.ReadAllBytesAsync(filename));

      if (projectQuality != null)
      {
        var projectQualityVM = new ProjectQualityVM(projectQuality);
        projectQualityVM.FileName = filename;
        var questView = new QuestView(projectQualityVM);
        CommandCenter.ExecuteCommand(WindowCommands.OpenWindow, new WindowOpenData(questView, "Quest #", filename));
        return projectQualityVM;
      }

    }
    catch (Exception e)
    {
      MessageBox.Show(e.Message);
    }
    return null;
  }

}
