using Syncfusion.UI.Xaml.Grid;

using ColorConverter = System.Windows.Media.ColorConverter;

namespace QuestWPF.Helpers;

/// <summary>
/// Helper methods to reflect properties from SfDataGrid and SfTreeGrid.
/// </summary>
public static class DataGridReflectorHelper
{

  /// <summary>
  /// Gets the text representation of a SfTreeGrid.
  /// Fills the provided DataObject with the text data.
  /// </summary>
  /// <param name="grid"></param>
  /// <param name="data"></param>
  public static void GetGridAsText(SfGridBase grid, DataObject data)
  {
    var lines = new List<string>();
    var line = new List<string>();
    foreach (var column in GetColumns(grid))
    {
      line.Add(column.HeaderText);
    }
    lines.Add(String.Join("\t", line));
    line.Clear();
    foreach (var row in GetRows(grid))
    {

      foreach (var column in GetColumns(grid))
      {
        var cellValue = GetCellValue(column, row);
        line.Add(cellValue);
      }
      lines.Add(String.Join("\t", line));
      line.Clear();
    }

    var str = String.Join("\n", lines);
    data.SetText(str);
  }


  /// <summary>
  /// Gets the HTML representation of a SfTreeGrid.
  /// Fills the provided DataObject with the HTML data.
  /// </summary>
  /// <param name="grid"></param>
  /// <param name="data"></param>
  public static void GetGridAsHtml(SfGridBase grid, DataObject data)
  {
    var html = new StringBuilder();
    // Get header colors from grid styles
    var (headerBackground, headerForeground) = DataGridReflectorHelper.GetHeaderColors(grid);

    // Build the HTML content
    html.AppendLine("<html>");
    html.AppendLine("<head>");
    html.AppendLine("<style>");
    html.AppendLine("table { border-collapse: collapse; font-family: Arial, sans-serif; font-size: 10pt; }");
    html.AppendLine($"th {{ background-color: {headerBackground}; color: {headerForeground}; font-weight: bold; padding: 3px; border: 1px solid #808080; text-align: center }}");
    html.AppendLine("td { padding: 5px; border: 1px solid #808080; }");
    html.AppendLine("tr:nth-child(even) { background-color: #F0F0F0; }");
    html.AppendLine(".numeric { text-align: right; }");
    html.AppendLine(".boolean { text-align: center; }");
    html.AppendLine("</style>");
    html.AppendLine("</head>");
    html.AppendLine("<body>");
    html.AppendLine("<!--StartFragment-->");
    html.AppendLine("<table>");

    // Add header row
    html.AppendLine("<thead>");
    html.AppendLine("<tr>");
    foreach (var column in GetColumns(grid))
    {
      html.Append("<th>");
      html.Append(System.Net.WebUtility.HtmlEncode(column.HeaderText));
      html.AppendLine("</th>");
    }
    html.AppendLine("</tr>");
    html.AppendLine("</thead>");

    // Add data rows
    html.AppendLine("<tbody>");
    foreach (var row in GetRows(grid))
    {
      // Get background and foreground colors from the row data
      string? rowStyle = (row is IQualityNodeVM viewModel) ? GetRowStyle(viewModel) : null;
      html.Append("<tr");
      if (!string.IsNullOrEmpty(rowStyle))
      {
        html.Append($" style=\"{rowStyle}\"");
      }
      html.AppendLine(">");
      foreach (var column in GetColumns(grid))
      {
        var cellValue = GetCellValue(column, row);
        if (IsNumericColumn(column))
          html.Append("<td class=\"numeric\">");
        else if (IsBooleanColumn(column))
        {
          html.Append("<td class=\"boolean\">");
          if (bool.TryParse(cellValue, out bool boolValue))
          {
            cellValue = BooleanValueToString(boolValue);
          }
        }
        else
          html.Append("<td>");

        html.Append(System.Net.WebUtility.HtmlEncode(cellValue));
        html.AppendLine("</td>");
      }
      html.AppendLine("</tr>");
    }
    html.AppendLine("</tbody>");

    html.AppendLine("</table>");
    html.AppendLine("<!--EndFragment-->");
    html.AppendLine("</body>");
    html.AppendLine("</html>");

    // Convert to HTML Clipboard Format required by Office applications
    string htmlContent = html.ToString();
    string htmlClipboardFormat = HtmlClipboardHelper.ConvertToHtmlClipboardFormat(htmlContent);
    // Set HTML data in clipboard
    data.SetData(DataFormats.Html, htmlClipboardFormat);
  }

  private static IEnumerable<GridColumnBase> GetColumns(SfGridBase grid)
  {
    if (grid is SfDataGrid dataGrid)
      return dataGrid.Columns;
    if (grid is SfTreeGrid treeGrid)
      return treeGrid.Columns;
    throw new NotImplementedException($"GetColumns not implemented for {grid.GetType()}");
  }

  private static IEnumerable<object> GetRows(SfGridBase grid)
  {
    if (grid is SfDataGrid dataGrid)
      return dataGrid.View.Records.Select(record => record.Data);
    if (grid is SfTreeGrid treeGrid)
      return treeGrid.View.Nodes.Select(node => node.Item);
    throw new NotImplementedException($"GetRows not implemented for {grid.GetType()}");
  }

  /// <summary>
  /// Gets the header background and foreground colors from the grid's visual tree.
  /// </summary>
  /// <param name="grid">The SfDataGrid instance.</param>
  /// <returns>A tuple containing the background and foreground colors as HTML color strings.</returns>
  public static (string background, string foreground) GetHeaderColors(FrameworkElement grid)
  {
    try
    {
      // Try to find the header cell visual in the grid's visual tree
      var headerCell = FindVisualChild<GridHeaderCellControl>(grid);
      if (headerCell != null)
      {
        var background = headerCell.Background;
        var foreground = headerCell.Foreground;

        if (background is SolidColorBrush bgBrush)
        {
          string bgColor = ColorToHex(bgBrush.Color);
          string fgColor = "#404040"; // Default foreground

          if (foreground is SolidColorBrush fgBrush)
          {
            fgColor = ColorToHex(fgBrush.Color);
          }

          return (bgColor, fgColor);
        }
      }

      // Fallback: try to get from application resources
      if (Application.Current.TryFindResource(typeof(GridHeaderCellControl)) is Style headerStyle)
      {
        string bgColor = "#C0C0C0"; // Default Silver
        string fgColor = "#404040";  // Default dark gray

        foreach (var setter in headerStyle.Setters.OfType<Setter>())
        {
          if (setter.Property == Control.BackgroundProperty && setter.Value is SolidColorBrush bgBrush)
          {
            bgColor = ColorToHex(bgBrush.Color);
          }
          else if (setter.Property == Control.ForegroundProperty && setter.Value is SolidColorBrush fgBrush)
          {
            fgColor = ColorToHex(fgBrush.Color);
          }
        }

        return (bgColor, fgColor);
      }
    }
    catch (Exception ex)
    {
      Debug.WriteLine($"Error getting header colors: {ex.Message}");
    }

    // Default colors matching SfTreeGridTools.xaml style
    return ("#C0C0C0", "#404040");
  }

  /// <summary>
  /// Gets the inline CSS style for a row based on its background color.
  /// </summary>
  /// <param name="viewModel">The record object view model</param>
  /// <returns>CSS style string with background and foreground colors, or null if no color is defined.</returns>
  public static string? GetRowStyle(IQualityNodeVM viewModel)
  {
    if (!string.IsNullOrEmpty(viewModel.BackgroundColor))
    {
      string backgroundColor = viewModel.BackgroundColor;

      // Parse the color to determine if it's dark or light
      try
      {
        var mediaColor = (Color)ColorConverter.ConvertFromString(backgroundColor);
        string foregroundColor = IsColorDark(mediaColor) ? "#FFFFFF" : "#000000";

        return $"background-color: {backgroundColor}; color: {foregroundColor};";
      }
      catch
      {
        // If color parsing fails, return null
        return null;
      }
    }
    return null;
  }

  /// <summary>
  /// Determines if a color is dark based on its luminance.
  /// </summary>
  /// <param name="color">The color to check.</param>
  /// <returns>True if the color is dark, false otherwise.</returns>
  public static bool IsColorDark(Color color)
  {
    // Calculate luminance using the same formula as QuestRowStyleSelector
    double luminance = (0.299 * color.R + 0.587 * color.G + 0.114 * color.B) / 255;
    return luminance < 0.5; // Dark if luminance is less than 0.5
  }

  /// <summary>
  /// Gets the header background and foreground colors from the grid's visual tree.
  /// </summary>
  /// <param name="grid">The SfTreeGrid instance.</param>
  /// <returns>A tuple containing the background and foreground colors as HTML color strings.</returns>
  public static (string background, string foreground) GetHeaderColors(SfTreeGrid grid)
  {
    try
    {
      // Try to find the header cell visual in the grid's visual tree
      var headerCell = FindVisualChild<GridHeaderCellControl>(grid);
      if (headerCell != null)
      {
        var background = headerCell.Background;
        var foreground = headerCell.Foreground;

        if (background is SolidColorBrush bgBrush)
        {
          string bgColor = ColorToHex(bgBrush.Color);
          string fgColor = "#404040"; // Default foreground

          if (foreground is SolidColorBrush fgBrush)
          {
            fgColor = ColorToHex(fgBrush.Color);
          }

          return (bgColor, fgColor);
        }
      }

      // Fallback: try to get from application resources
      if (Application.Current.TryFindResource(typeof(GridHeaderCellControl)) is Style headerStyle)
      {
        string bgColor = "#C0C0C0"; // Default Silver
        string fgColor = "#404040";  // Default dark gray

        foreach (var setter in headerStyle.Setters.OfType<Setter>())
        {
          if (setter.Property == Control.BackgroundProperty && setter.Value is SolidColorBrush bgBrush)
          {
            bgColor = ColorToHex(bgBrush.Color);
          }
          else if (setter.Property == Control.ForegroundProperty && setter.Value is SolidColorBrush fgBrush)
          {
            fgColor = ColorToHex(fgBrush.Color);
          }
        }

        return (bgColor, fgColor);
      }
    }
    catch (Exception ex)
    {
      Debug.WriteLine($"Error getting header colors: {ex.Message}");
    }

    // Default colors matching SfTreeGridTools.xaml style
    return ("#C0C0C0", "#404040");
  }

  /// <summary>
  /// Finds a child visual element of the specified type in the visual tree.
  /// </summary>
  /// <typeparam name="T">The type of visual element to find.</typeparam>
  /// <param name="parent">The parent element to start the search from.</param>
  /// <returns>The first child element of the specified type, or null if not found.</returns>
  public static T? FindVisualChild<T>(DependencyObject? parent) where T : DependencyObject
  {
    if (parent == null)
      return null;

    int childCount = VisualTreeHelper.GetChildrenCount(parent);
    for (int i = 0; i < childCount; i++)
    {
      var child = VisualTreeHelper.GetChild(parent, i);
      if (child is T typedChild)
        return typedChild;

      var result = FindVisualChild<T>(child);
      if (result != null)
        return result;
    }

    return null;
  }

  /// <summary>
  /// Converts a Color to a hex string format (#RRGGBB).
  /// </summary>
  /// <param name="color">The color to convert.</param>
  /// <returns>A hex color string.</returns>
  public static string ColorToHex(Color color)
  {
    return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
  }

  /// <summary>
  /// Gets the cell value for a specific column from a tree grid node.
  /// </summary>
  /// <param name="column">The grid column.</param>
  /// <param name="data">Row data object.</param>
  /// <returns>The formatted cell value as a string.</returns>
  public static string GetCellValue(GridColumnBase column, object? data)
  {
    if (data == null)
      return string.Empty;

    var mappingName = GetMappingName(column, data);

    if (string.IsNullOrEmpty(mappingName))
      return string.Empty;

    // Get the property value using reflection
    var propertyInfo = data.GetType().GetProperty(mappingName);
    if (propertyInfo == null)
      return string.Empty;

    var value = propertyInfo.GetValue(data);
    if (value == null)
      return string.Empty;

    // Format numeric values according to column settings
    var format = GetNumericColumnFormat(column);
    if (format!=null && value is double doubleValue)
      return doubleValue.ToString("F2", System.Globalization.CultureInfo.CurrentCulture);

    return value.ToString() ?? string.Empty;
  }

  /// <summary>
  /// Checks whether a column is a numeric column.
  /// </summary>
  /// <param name="column"></param>
  /// <returns></returns>
  private static bool IsNumericColumn(GridColumnBase column)
  {
    return (column is GridNumericColumn) || (column is TreeGridNumericColumn) || column.MappingName == "Value";
  }

  /// <summary>
  /// Returns numeric column format or null;
  /// </summary>
  /// <param name="column"></param>
  /// <returns></returns>
  private static string? GetNumericColumnFormat(GridColumnBase column)
  {
    if (column is GridNumericColumn numericColumn1)
      return $"F{numericColumn1.NumberDecimalDigits}";
    if (column is TreeGridNumericColumn numericColumn2)
      return $"F{numericColumn2.NumberDecimalDigits}";
    if (column.MappingName == "Value")
      return "F2";
    return null;
  }

  /// <summary>
  /// Checks whether a column is a boolean column.
  /// </summary>
  /// <param name="column"></param>
  /// <returns></returns>
  private static bool IsBooleanColumn(GridColumnBase column)
  {
    return (column is GridCheckBoxColumn) || (column is TreeGridTemplateColumn);
  }

  /// <summary>
  /// Converts a boolean value to a string representation using Unicode characters.
  /// </summary>
  /// <param name="value"></param>
  /// <returns></returns>
  public static string? BooleanValueToString(bool value)
  {
    return value ? "\u2611" : "\u2610";
  }

  /// <summary>
  /// Gets the mapping name for a GridColumnBase.
  /// </summary>
  /// <param name="column"></param>
  /// <param name="dataItem"></param>
  /// <returns></returns>
  private static string? GetMappingName(GridColumnBase column, object dataItem)
  {
    if (column.CellTemplateSelector is not null)
      return column.CellTemplateSelector.GetMappingName(dataItem, column);
    return column.MappingName;
  }
}