namespace QuestWPF.Helpers;

/// <summary>
/// Selector for choosing the appropriate DataTemplate for quest view content.
/// </summary>
public class QuestViewContentTemplateSelector: DataTemplateSelector
{
  /// <summary>
  /// Method to select the appropriate DataTemplate based on the item and container.
  /// </summary>
  /// <param name="item"></param>
  /// <param name="container"></param>
  /// <returns></returns>
  public override DataTemplate? SelectTemplate(object? item, DependencyObject container)
  {
    if (item is QuestItemViewModel questItemView)
    {
      if (questItemView.Content is QualityScaleVM)
        return QualityScaleTemplate;
      if (questItemView.Content is DocumentQualityVM)
        return DocumentQualityTemplate;
    }
    return base.SelectTemplate(item, container);
  }

  /// <summary>
  /// Template for displaying a quality scale.
  /// </summary>
  public DataTemplate? QualityScaleTemplate { get; set; }

  /// <summary>
  /// Template for displaying document quality details.
  /// </summary>
  public DataTemplate? DocumentQualityTemplate { get; set; }
}