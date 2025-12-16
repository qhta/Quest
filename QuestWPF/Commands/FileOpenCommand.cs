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
        await OpenQxmlFile(filename);
      }

    }
    catch (Exception e)
    {
      MessageBox.Show(e.Message);
    }
  }

  /// <summary>
  /// Opens an Qxml file given its filename.
  /// First, it deserializes the file into a ProjectQuality object. 
  /// Next, it creates a ProjectQualityVM from the ProjectQuality object
  /// and then opens a new QuestView window to display the project.
  /// </summary>
  /// <param name="filename"></param>
  /// <returns></returns>
  public async Task<ProjectQualityVM?> OpenQxmlFile(string filename)
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
