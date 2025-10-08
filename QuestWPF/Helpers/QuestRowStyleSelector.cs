namespace QuestWPF.Helpers
{
  /// <summary>
  /// Selects a style for TreeGrid rows based on the BackgroundColor of IQualityNodeVM BackgroundColor property.
  /// </summary>
  public class QuestRowStyleSelector : StyleSelector
  {
    /// <summary>
    /// Selects a style for TreeGrid rows based on the BackgroundColor of IQualityNodeVM BackgroundColor property.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="container"></param>
    /// <returns></returns>
    public override Style? SelectStyle(object item, DependencyObject container)
    {
      if (item is TreeDataRow treeDataRow && treeDataRow.RowData is IQualityNodeVM node)
      {
        // Convert System.Drawing.Color to System.Windows.Media.Color
        var backgroundColor = node.BackgroundColor;
        if (backgroundColor != null)
        {
          var mediaColor = backgroundColor.StartsWith("#")?
            (Color)System.Windows.Media.ColorConverter.ConvertFromString(backgroundColor) :
            GetColorByName(backgroundColor);
          var style = new Style(typeof(TreeGridRowControl));
          style.Setters.Add(new Setter(Control.BackgroundProperty, new SolidColorBrush(mediaColor)));
          return style;
        }
      }

      return base.SelectStyle(item, container);
    }

    private static Color GetColorByName(string colorName)
    {
      var colorProperty = typeof(Colors).GetProperty(colorName);
      if (colorProperty != null)
      {
        var colorValue = colorProperty.GetValue(null);
        if (colorValue != null)
          return (Color)colorValue;
      }
      throw new ArgumentException($"Color '{colorName}' is not a valid System.Windows.Media.Color.");
    }
  }
}