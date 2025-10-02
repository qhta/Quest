namespace QuestWPF.Helpers;

/// <summary>
/// Selector for choosing the appropriate DataTemplate for quality node value.
/// </summary>
public class QualityNodeValueTemplateSelector: DataTemplateSelector
{
  /// <summary>
  /// Method to select the appropriate DataTemplate based on the item and container.
  /// </summary>
  /// <param name="item"></param>
  /// <param name="container"></param>
  /// <returns></returns>
  public override DataTemplate? SelectTemplate(object? item, DependencyObject container)
  {
    if (item is QualityMeasureVM)
      return QualityMeasureTemplate;
    if (item is QualityMetricsVM)
      return QualityMetricsTemplate;
    if (item is QualityFactorVM)
      return QualityFactorTemplate;
    return base.SelectTemplate(item, container);
  }

  /// <summary>
  /// Template for displaying a quality measure grade.
  /// </summary>
  public DataTemplate? QualityMeasureTemplate { get; set; }

  /// <summary>
  /// Template for displaying document metrics value.
  /// </summary>
  public DataTemplate? QualityMetricsTemplate { get; set; }

  /// <summary>
  /// Template for displaying document Factor value.
  /// </summary>
  public DataTemplate? QualityFactorTemplate { get; set; }
}