using System.ComponentModel.DataAnnotations.Schema;

namespace QuestRDM;

/// <summary>
/// Information about a project.
/// </summary>
public class Project
{
  /// <summary>
  /// The ID of the project.
  /// </summary>
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public int ID { get; set; }

  /// <summary>
  /// Global Unique Identifier for the project. Enables linking from QuestQDM.
  /// </summary>
  public Guid Guid { get; set; }

  /// <summary>
  /// Title of the project.
  /// </summary>
  [MaxLength(255)]
  public string? Title { get; set; }
}