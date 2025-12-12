namespace Quest;

/// <summary>
/// Grade definition with text, numeric value, and optional meaning.
/// </summary>
public class QualityGrade()
{
  /// <summary>
  /// Unique identifier for the grade.
  /// </summary>
  public int Id { get; set; }

  /// <summary>
  /// Text representation of the grade, e.g. "1", "2", "b.o."
  /// </summary>
  [MaxLength(4)]
  public string Text { get; set; } = null!;

  /// <summary>
  /// Numeric value of the grade, e.g. 1, 2, 0 (for "b.o.")
  /// </summary>
  public int Value { get; set; }

  /// <summary>
  /// Is this grade considered reliable?
  /// Values marked as unreliable may be treated differently in evaluations.
  /// </summary>
  public bool Reliable { get; set; } = true;

  /// <summary>
  /// Textual meaning of the grade.
  /// </summary>
  [MaxLength(255)]
  public string? Meaning { get; set; }
}