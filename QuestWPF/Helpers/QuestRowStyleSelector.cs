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
          var mediaColor = (Color)System.Windows.Media.ColorConverter.ConvertFromString(backgroundColor);
          var style = new Style(typeof(TreeGridRowControl));
          style.Setters.Add(new Setter(Control.BackgroundProperty, new SolidColorBrush(mediaColor)));
          if (IsColorDark(mediaColor))
          {
            style.Setters.Add(new Setter(Control.ForegroundProperty, Brushes.White));
          }
          else
          {
            style.Setters.Add(new Setter(Control.ForegroundProperty, Brushes.Black));
          }
          return style;
        }
      }

      return base.SelectStyle(item, container);
    }

    private static bool IsColorDark(Color color)
    {
      // Calculate luminance
      double luminance = (0.299 * color.R + 0.587 * color.G + 0.114 * color.B) / 255;
      return luminance < 0.5; // Dark if luminance is less than 0.5
    }

  }
}