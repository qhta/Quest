using System.ComponentModel.DataAnnotations.Schema;

namespace Quest;

/// <summary>
/// Quality of a project phase.
/// </summary>
public class PhaseQuality: Quality
{
  /// <summary>
  /// Phase Id.
  /// </summary>
  [MaxLength(10)]
  public string? PhaseId { get; set; }

  /// <summary>
  /// Name or path of the document.
  /// </summary>
  [MaxLength(255)]
  public string? PhaseName { get; set; }

  /// <summary>
  /// Foreign key referencing the associated ProjectQuality.
  /// </summary>
  public int ProjectQualityId { get; set; }

  /// <summary>
  /// Is the document required for the project?
  /// </summary>
  public bool IsRequired { get; set; }

  /// <summary>
  /// Is the document available for assessment?
  /// </summary>
  public bool IsAvailable { get; set; }

  /// <summary>
  /// Is the document assessed?
  /// </summary>
  public bool IsAssessed { get; set; }

  /// <summary>
  /// Gets or sets the collection of quality factors associated with the current context.
  /// </summary>
  public List<QualityFactorAggregate> Factors { get; set; } = new List<QualityFactorAggregate>();


  /// <summary>
  /// A collection of document qualities within the project.
  /// </summary>
  public List<DocumentQuality>? DocumentQualities { get; set; }
}