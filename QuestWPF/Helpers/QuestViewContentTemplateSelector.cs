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
    if (item is QualityScaleVM)
    {
      return Application.Current.Resources["QualityScaleTemplate"] as DataTemplate;
    }

    return base.SelectTemplate(item, container);
  }
}