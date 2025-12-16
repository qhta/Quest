using System.Text;

using Qhta.Xml.Serialization;

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
    if (CurrentProject == null)
      throw new InvalidOperationException("No project loaded");

    return await Task.Run(() =>
    {
      using var memoryStream = new MemoryStream();
      using (var writer = new StreamWriter(memoryStream, Encoding.UTF8, leaveOpen: true))
      {
        var xmlSerializer = new QXmlSerializer(typeof(ProjectQuality));
        xmlSerializer.Serialize(writer, CurrentProject.Model);
      }
      return memoryStream.ToArray();
    });
  }

  /// <summary>
  /// Deserializes project from XML data
  /// </summary>
  public async Task<ProjectQuality?> DeserializeProjectAsync(byte[] data)
  {
    return await Task.Run(() =>
    {
      using var memoryStream = new MemoryStream(data);
      using var reader = new StreamReader(memoryStream);
      var xmlSerializer = new QXmlSerializer(typeof(ProjectQuality));
      return xmlSerializer.Deserialize(reader) as ProjectQuality;
    });
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
      var projectQuality = await DeserializeProjectAsync(fileData);
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