namespace Quest;

/// <summary>
/// Top level quality factor with weight and grade value.
/// </summary>
public class QualityFactor: QualityMetricsNode
{
  ///// <summary>
  ///// Foreign key referencing the associated QualityFactorType.
  ///// </summary>
  //public int? TypeId { get; set; }

  /// <summary>
  /// Navigation property to the associated QualityFactorType.
  /// </summary>
  public QualityFactorType? FactorType { get; set; }

  /// <summary>
  /// Navigation property to the associated DocumentQuality.
  /// </summary>
  public DocumentQuality DocumentQuality { get; set; } = null!;
}