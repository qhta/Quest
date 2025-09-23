namespace Quest;

/// <summary>
/// Leaf node representing a quality assessment.
/// </summary>
public interface IQualityAssessment
{
  /// <summary>
  /// Navigation property to the parent quality assessment.
  /// </summary>
  [Required]
  public IQualityAssessment Assessment { get; set;}

  /// <summary>
  /// Foreign key to the Evaluator person.
  /// </summary>
  public int EvaluatorId { get; set; }

  /// <summary>
  /// Assessed grade value.
  /// </summary>
  public Grade? Value { get; set; }
}