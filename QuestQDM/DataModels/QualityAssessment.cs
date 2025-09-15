namespace Quest;

/// <summary>
/// Leaf node representing a quality assessment.
/// </summary>
public class QualityAssessment: QualityNode
{
  /// <summary>
  /// Assessed grade value.
  /// </summary>
  public Grade? Value { get; set; }
}