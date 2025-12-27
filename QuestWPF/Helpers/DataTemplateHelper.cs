namespace QuestWPF.Helpers;

/// <summary>
/// Helper methods for DataTemplate and BindingExpression classes.
/// </summary>
public static class DataTemplateHelper
{
  /// <summary>
  /// Gets the mapping name from a DataTemplateSelector.
  /// </summary>
  public static string? GetMappingName(this DataTemplateSelector templateSelector, object dataItem, DependencyObject container)
  {
    var dataTemplate = templateSelector.SelectTemplate(dataItem, container);
    if (dataTemplate == null) return null;
    var obj = dataTemplate?.LoadContent();
    if (obj is TextBlock textBlock)
      return textBlock.GetBindingExpression(TextBlock.TextProperty).GetMappingName();
    if (obj is TextBox textBox)
      return textBox.GetBindingExpression(TextBox.TextProperty).GetMappingName();
    if (obj is ComboBox comboBox)
      return comboBox.GetBindingExpression(ComboBox.TextProperty).GetMappingName();
    return null;
  }

  /// <summary>
  /// Retrieves the property path associated with the specified binding expression, if available.
  /// </summary>
  /// <param name="bindingExpression">The binding expression from which to extract the property path. Can be null.</param>
  /// <returns>The property path as a string if the binding expression and its path are available; otherwise, null.</returns>
  public static string? GetMappingName(this BindingExpression? bindingExpression)
  {
    if (bindingExpression?.ParentBinding.Path != null)
    {
      return bindingExpression.ParentBinding.Path.Path;
    }
    return null;
  }

}