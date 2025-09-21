namespace Quest;

/// <summary>
/// Aggregated value of quality factors.
/// </summary>
public class QualityFactorAggregate: QualityNode
{
  /// <summary>
  /// Aggregated factor type.
  /// </summary>
  public QualityFactorType FactorType { get; set; }

  /// <summary>
  /// Aggregated value.
  /// </summary>
  public double? Value { get; set; }
}