namespace Quest;

/// <summary>
/// Composite quality metrics containing child nodes. Its value is computed from the child nodes.
/// </summary>
public interface IQualityMetrics: IQualityMetricsNode
{
  /// <summary>
  /// Parent node in the quality metrics hierarchy. Top metrics have factor as parents.
  /// </summary>
  public IQualityMetricsNode Parent { get; set; }
}