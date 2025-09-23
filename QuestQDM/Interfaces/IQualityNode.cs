using System.ComponentModel.DataAnnotations.Schema;

namespace Quest;

/// <summary>
/// Represents an abstract node of a quality tree.
/// </summary>
public interface IQualityNode
{
  /// <summary>
  ///  Level of the quality node in the tree (0 for root, 1 for factor).
  /// </summary>
  public int Level { get; set; }

  /// <summary>
  /// Order of the quality node among its siblings.
  /// </summary>
  public int Ord { get; set; }

  /// <summary>
  /// Display text of the quality node.
  /// </summary>
  [MaxLength(255)]
  public string? Text { get; set; }

  /// <summary>
  /// Weight of the quality value in overall assessment.
  /// </summary>
  public int Weight { get; set; }

  /// <summary>
  /// A short comment or description about the quality node.
  /// </summary>
  [MaxLength(255)]
  public string? Comment { get; set; }
}