namespace Quest;

/// <summary>
/// Scale of grades for quality assessment.
/// </summary>
public enum Grade
{
  /// <summary>
  /// No grade assigned
  /// </summary>
  none = 0,
  lack = 1,
  poor = 2,
  fair = 3,
  good = 4,
  excellent = 5,
  perfect = 6,
  /// <summary>
  /// Not applicable
  /// </summary>
  n_a = -1,
  /// <summary>
  /// No requirement
  /// </summary>
  n_r = -2,
}