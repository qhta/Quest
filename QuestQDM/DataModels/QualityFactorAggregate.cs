namespace Quest;

/// <summary>
/// Aggregated value of quality factors.
/// </summary>
public class QualityFactorAggregate
{
  /// <summary>
  /// Unique identifier for the entity.
  /// </summary>
  public int Id { get; set; }

  /// <summary>
  /// Aggregated factor type.
  /// </summary>
  public QualityFactorType FactorType { get; set; }

  /// <summary>
  /// Aggregated value.
  /// </summary>
  public double? Value { get; set; }
}