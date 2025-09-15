namespace Quest;

/// <summary>
/// Composite quality metrics containing child nodes. Its value is computed from the child nodes.
/// </summary>
public class Metrics: QualityNode
{
  /// <summary>
  /// Gets or sets the collection of child nodes associated with this node.
  /// </summary>
  public IEnumerable<MetricsNode>? Children { get; set; }
}