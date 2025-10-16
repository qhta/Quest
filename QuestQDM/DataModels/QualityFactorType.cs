namespace Quest;

/// <summary>
/// Definition of quality factor type.
/// </summary>
public class QualityFactorType
{
  /// <summary>
  /// Numeric identifier.
  /// </summary>
  public int Id { get; set; }

  /// <summary>
  /// Name of the quality factor type.
  /// </summary>
  [MaxLength(20)]
  public string? Name { get; set; }

  /// <summary>
  /// Background color associated with the quality factor type (for UI purposes).
  /// </summary>
  [MaxLength(20)]
  public string[]? Colors { get; set; }

}