using System.Collections.Generic;

namespace Quest;

/// <summary>
/// Singleton ViewModel that holds a ProjectsCollection.
/// </summary>
public class ProjectsViewModel
{

  readonly QuestRdmDbContext dbContext = new QuestRdmDbContext();
  private static readonly Lazy<ProjectsViewModel> _instance = new(() => new ProjectsViewModel());

  /// <summary>
  /// Gets the singleton instance of the ProjectsViewModel.
  /// </summary>
  public static ProjectsViewModel Instance => _instance.Value;

  /// <summary>
  /// The ProjectsCollection managed by this ViewModel.
  /// </summary>
  public ProjectsCollection Projects { get; private set; }

  /// <summary>
  /// Private constructor to enforce singleton pattern.
  /// </summary>
  private ProjectsViewModel()
  {
    // Initialize the ProjectsCollection with an empty list or fetch from a data source.
    Projects = new ProjectsCollection(new List<Project>());
  }

  /// <summary>
  /// Method to load projects from the QuestRDM.
  /// </summary>
  public void LoadProjects()
  {
    // Fetch projects from the database and populate the ProjectsCollection.
    var projects = dbContext.Projects.ToList();
    Projects = new ProjectsCollection(projects);
  }

}