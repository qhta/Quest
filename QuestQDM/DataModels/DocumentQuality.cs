namespace Quest;

/// <summary>
/// Quality of a single document.
/// </summary>
public class DocumentQuality
{
  /// <summary>
  /// Unique identifier for the quality.
  /// </summary>
  public int Id { get; set; }

  /// <summary>
  /// Type of the document.
  /// </summary>
  [MaxLength(255)]
  public string? DocumentType { get; set; }

  /// <summary>
  /// Name or path of the document.
  /// </summary>
  [MaxLength(255)]
  public string? DocumentTitle { get; set; }

  /// <summary>
  /// Foreign key referencing the associated ProjectQuality.
  /// </summary>
  public int ProjectQualityId { get; set; }

  public bool IsRequired { get; set; }

  public bool IsAvailable { get; set; }

  public bool IsAssessed { get; set; }

  /// <summary>
  /// Gets or sets the collection of quality factors associated with the current context.
  /// </summary>
  public List<QualityFactor> Factors { get; set; } = new List<QualityFactor>();
}