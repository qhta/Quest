using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quest;

/// <summary>
/// Base class for quality assessment.
/// </summary>
[HideInheritedMembers]
public abstract class QualityObject: ObservableObject
{
  /// <summary>
  /// Unique identifier for the quality.
  /// </summary>
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  [DefaultValue(0)]
  public int Id { get; set; }

}