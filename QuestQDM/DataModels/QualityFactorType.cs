namespace Quest;

/// <summary>
/// Available types of quality factors.
/// </summary>
public enum QualityFactorType: byte
{
  /// <summary>
  /// Not specified.
  /// </summary>
  None = 0,
  /// <summary>
  /// Specifies the degree to which all necessary parts are present.
  /// </summary>
  Completeness = 1,
  /// <summary>
  /// Specifies the degree to which the information is correct and free from errors.
  /// </summary>
  Correctness = 2,
  /// <summary>
  /// Consistency refers to the uniformity and coherence of information across different parts of a system or document.
  /// </summary>
  Consistency = 3,
  /// <summary>
  /// Specifies how easily information can be understood by the intended audience.
  /// </summary>
  Understandability = 4,
  /// <summary>
  /// Specifies how easily information can be changed or adapted when necessary.
  /// </summary>
  Modifiability = 5,
  /// <summary>
  /// Specifies the degree to which information can be verified for completeness and correctness.
  /// </summary>
  Verifiability = 6,
}