namespace Quest;

/// <summary>
/// Quality metrics node in a hierarchical structure of quality nodes.
/// </summary>
public class QualityMetricsNode: QualityNode
{
  /// <summary>
  /// Gets or sets the parent node associated with this quality metrics node.
  /// </summary>
  public QualityMetricsNode Parent { get; set; } = null!;

  /// <summary>
  /// Gets or sets the collection of child nodes associated with this node.
  /// </summary>
  public List<QualityNode> Children { get; set; } = new List<QualityNode>();

  /// <summary>
  /// Evaluated value computed from child nodes.
  /// </summary>
  public double? Value { get; set; }
}