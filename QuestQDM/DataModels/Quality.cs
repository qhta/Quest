using System.ComponentModel.DataAnnotations.Schema;

namespace Quest;

/// <summary>
/// Base class for quality assessment.
/// </summary>
public abstract class Quality
{
  /// <summary>
  /// Unique identifier for the quality.
  /// </summary>
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public int Id { get; set; }

}