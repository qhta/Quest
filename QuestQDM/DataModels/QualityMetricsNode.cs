namespace Quest;

/// <summary>
/// Quality metrics node in a hierarchical structure of quality nodes.
/// </summary>
public class QualityMetricsNode: QualityNode
{
  /// <summary>
  /// Navigation property to the parent node. Null for Factor node.
  /// </summary>
  public QualityMetricsNode? Parent { get; set; }
  /// <summary>
  /// Gets or sets the collection of child nodes associated with this node.
  /// </summary>
  public List<QualityNode> Children { get; set; } = new List<QualityNode>();
}