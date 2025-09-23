using System.ComponentModel.DataAnnotations.Schema;

namespace Quest;

/// <summary>
/// Base class for quality assessment.
/// </summary>
public abstract class Quality
{
  /// <summary>
  /// Unique identifier for the quality.
  /// </summary>
  public int Id { get; set; }

  /// <summary>
  /// Identifies the name of the quality context (e.g., project name, document name).
  /// </summary>
  public string? Name { get; set; }

  /// <summary>
  /// Gets or sets the collection of quality factors associated with the current context.
  /// </summary>
  public List<QualityFactor> Factors { get; set; } = new List<QualityFactor> ();

}