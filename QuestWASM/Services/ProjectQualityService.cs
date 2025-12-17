using Quest;

namespace QuestWASM;

public class ProjectQualityService
{
  public ProjectQualityVM? CurrentProject { get; private set; }
  public string? CurrentFileName { get; private set; }

  public event Action? OnProjectChanged;

  public void LoadProject(ProjectQuality projectQuality, string? fileName)
  {
    CurrentProject = new ProjectQualityVM(projectQuality);
    CurrentProject.Evaluate();
    CurrentProject.IsExpanded = true;
    CurrentFileName = fileName;

    OnProjectChanged?.Invoke();
  }

  public void ClearProject()
  {
    CurrentProject = null;
    CurrentFileName = null;
    OnProjectChanged?.Invoke();
  }

  /// <summary>
  /// Serializes the current project to XML format
  /// </summary>
  public async Task<byte[]> SerializeProjectAsync()
  {
    return await FileCommandHelper.SerializeProjectAsync(CurrentProject!.Model);
  }

  /// <summary>
  /// Checks if project can be saved
  /// </summary>
  public bool CanSave()
  {
    return CurrentProject != null;
  }


  /// <summary>
  /// Checks if project can not be saved
  /// </summary>
  public bool CannotSave()
  {
    return CurrentProject == null;
  }

  /// <summary>
  /// Opens and loads a project from file data
  /// </summary>
  public async Task<bool> OpenProjectAsync(string fileName, byte[] fileData)
  {
    try
    {
      var projectQuality = Path.GetExtension(fileName).ToLower().EndsWith("xml") ?
                await FileCommandHelper.DeserializeProjectAsync(fileData) :
                await FileCommandHelper.UnpackProjectAsync(fileData);
      if (projectQuality != null)
      {
        LoadProject(projectQuality, fileName);
        return true;
      }
      return false;
    }
    catch
    {
      return false;
    }
  }
}