namespace Quest;

/// <summary>
/// Quality measure containing assessments. Its value is computed from the assessments.
/// </summary>
public class Measure: MetricsNode
{
  public IEnumerable<QualityAssessment>? Assessments { get; set; }
}