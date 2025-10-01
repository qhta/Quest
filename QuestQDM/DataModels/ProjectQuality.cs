namespace Quest;

/// <summary>
/// Quality of the entire project.
/// </summary>
public class ProjectQuality: QualityObject
{
  ///// <summary>
  ///// Unique identifier for the entity.
  ///// </summary>
  //public int Id { get; set; }

  /// <summary>
  /// Identifies the name of the assessed project name.
  /// </summary>
  [MaxLength(255)]
  public string? ProjectName { get; set; }

  /// <summary>
  /// Globally unique identifier for the assessed project.
  /// </summary>
  public Guid? ProjectId { get; set; }

  /// <summary>
  /// Gets or sets the scale used to evaluate quality grades.
  /// </summary>
  public List<QualityGrade>? Scale { get; set; }

  /// <summary>
  /// Gets or sets the collection of quality factors associated with the current context.
  /// </summary>
  public List<QualityFactorAggregate>? Factors { get; set; }

  /// <summary>
  /// A collection of phase qualities within the project.
  /// </summary>
  public List<PhaseQuality>? PhaseQualities { get; set; }

  /// <summary>
  /// A collection of document qualities within the project.
  /// </summary>
  public List<DocumentQuality>? DocumentQualities { get; set; }
}