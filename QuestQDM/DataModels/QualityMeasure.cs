namespace Quest;

/// <summary>
/// Quality measure containing assessments. Its value is computed from the assessments.
/// </summary>
public class QualityMeasure: QualityNode
{
  /// <summary>
  /// Agreed grade value.
  /// </summary>
  [MaxLength(10)]
  public string? Grade { get; set; }

  /// <summary>
  /// Evaluated value computed from child nodes.
  /// </summary>
  public override double? Value { get; set; }

  ///// <summary>
  ///// Collection of assessments contributing to this quality measure value.
  ///// </summary>
  //public List<QualityAssessment>? Assessments { get; set; }
}