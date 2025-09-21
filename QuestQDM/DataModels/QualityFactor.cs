namespace Quest;

/// <summary>
/// Top level quality factor with weight and grade value.
/// </summary>
public class QualityFactor: QualityMetrics
{
  /// <summary>
  /// Unique identifier for the quality factor.
  /// </summary>
  public QualityFactorType FactorType { get; set; }

  /// <summary>
  /// Foreign key referencing the associated DocumentQuality.
  /// </summary>
  public int DocumentQualityId { get; set; }

  /// <summary>
  /// Navigation property to the associated DocumentQuality.
  /// </summary>
  public DocumentQuality DocumentQuality { get; set; } = null!;
}