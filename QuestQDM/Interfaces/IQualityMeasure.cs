namespace Quest;

/// <summary>
/// Quality measure containing assessments. Its value is computed from the assessments.
/// </summary>
public interface IQualityMeasure: IQualityMetricsNode
{
  /// <summary>
  /// Agreed grade value.
  /// </summary>
  public Grade? Value { get; set; }

  /// <summary>
  /// Collection of assessments contributing to this quality measure value.
  /// </summary>
  public ICollection<IQualityAssessment>? Assessments { get; set; }
}