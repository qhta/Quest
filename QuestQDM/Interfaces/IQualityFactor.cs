namespace Quest;

/// <summary>
/// Top level quality factor with weight and grade value.
/// </summary>
public interface IQualityFactor: IQualityMetricsNode
{
  /// <summary>
  /// Unique identifier for the quality factor.
  /// </summary>
  public QualityFactorType FactorType { get; set; }

  /// <summary>
  /// Navigation property to the associated DocumentQuality.
  /// </summary>
  public DocumentQuality DocumentQuality { get; set; }
}