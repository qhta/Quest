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
}