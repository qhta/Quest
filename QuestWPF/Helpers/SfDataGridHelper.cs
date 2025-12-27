using Syncfusion.Data;
using Syncfusion.UI.Xaml.Grid;
using ColorConverter = System.Windows.Media.ColorConverter;
using GridColumn = Syncfusion.UI.Xaml.Grid.GridColumn;

namespace QuestWPF.Helpers;

/// <summary>
/// Helper methods for SfDataGrid.
/// </summary>
public static class SfDataGridHelper
{
  /// <summary>
  /// Gets the text representation of a SfDataGrid.
  /// Fills the provided DataObject with the text data.
  /// </summary>
  /// <param name="grid"></param>
  /// <param name="data"></param>
  public static void GetDataGridAsText(SfDataGrid grid, DataObject data)
  {
    var lines = new List<string>();
    var line = new List<string>();
    foreach (var column in grid.Columns)
    {
      line.Add(column.HeaderText);
    }
    lines.Add(String.Join("\t", line));
    line.Clear();
    foreach (var row in grid.View.Records)
    {

      foreach (var column in grid.Columns)
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
  /// Gets the HTML representation of a SfDataGrid.
  /// Fills the provided DataObject with the HTML data.
  /// </summary>
  /// <param name="grid"></param>
  /// <param name="data"></param>
  public static void GetDataGridAsHtml(SfDataGrid grid, DataObject data)
  {
    var html = new StringBuilder();
    // Get header colors from grid styles
    var (headerBackground, headerForeground) = GetHeaderColors(grid);

    // Build the HTML content
    html.AppendLine("<html>");
    html.AppendLine("<head>");
    html.AppendLine("<style>");
    html.AppendLine("table { border-collapse: collapse; font-family: Arial, sans-serif; font-size: 10pt; }");
    html.AppendLine($"th {{ background-color: {headerBackground}; color: {headerForeground}; font-weight: bold; padding: 3px; border: 1px solid #808080; text-align: center }}");
    html.AppendLine("td { padding: 5px; border: 1px solid #808080; }");
    html.AppendLine("tr:nth-child(even) { background-color: #F0F0F0; }");
    html.AppendLine(".numeric { text-align: right; }");
    html.AppendLine("</style>");
    html.AppendLine("</head>");
    html.AppendLine("<body>");
    html.AppendLine("<!--StartFragment-->");
    html.AppendLine("<table>");

    // Add header row
    html.AppendLine("<thead>");
    html.AppendLine("<tr>");
    foreach (var column in grid.Columns)
    {
      html.Append("<th>");
      html.Append(System.Net.WebUtility.HtmlEncode(column.HeaderText));
      html.AppendLine("</th>");
    }
    html.AppendLine("</tr>");
    html.AppendLine("</thead>");

    // Add data rows
    html.AppendLine("<tbody>");
    foreach (var row in grid.View.Records)
    {
      // Get background and foreground colors from the row data
      string? rowStyle = GetRowStyle(row);
      html.Append("<tr");
      if (!string.IsNullOrEmpty(rowStyle))
      {
        html.Append($" style=\"{rowStyle}\"");
      }
      html.AppendLine(">");
      foreach (var column in grid.Columns)
      {
        var cellValue = GetCellValue(column, row);
        var isNumeric = column is GridNumericColumn;

        html.Append(isNumeric ? "<td class=\"numeric\">" : "<td>");
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
    string htmlClipboardFormat = ConvertToHtmlClipboardFormat(htmlContent);
    // Set HTML data in clipboard
    data.SetData(DataFormats.Html, htmlClipboardFormat);
  }

  /// <summary>
  /// Converts HTML content to the HTML Clipboard Format required by Microsoft Office applications.
  /// </summary>
  /// <param name="html">The HTML content to convert.</param>
  /// <returns>HTML content with proper clipboard format headers.</returns>
  public static string ConvertToHtmlClipboardFormat(string html)
  {
    // Encode to UTF-8
    byte[] utf8Bytes = Encoding.UTF8.GetBytes(html);
    string utf8Html = Encoding.UTF8.GetString(utf8Bytes);

    // Build the clipboard format header
    StringBuilder sb = new StringBuilder();

    // Version
    sb.AppendLine("Version:0.9");

    // Calculate byte positions (header + content)
    string header = sb.ToString();
    int startHTML = header.Length + 100; // Approximate header size
    int endHTML = startHTML + utf8Bytes.Length;

    // Find fragment markers
    int startFragment = utf8Html.IndexOf("<!--StartFragment-->", StringComparison.Ordinal);
    int endFragment = utf8Html.IndexOf("<!--EndFragment-->", StringComparison.Ordinal);

    if (startFragment >= 0)
      startFragment = startHTML + startFragment + "<!--StartFragment-->".Length;
    else
      startFragment = startHTML;

    if (endFragment >= 0)
      endFragment = startHTML + endFragment;
    else
      endFragment = endHTML;

    // Build complete header
    sb.Clear();
    sb.AppendLine("Version:0.9");
    sb.AppendLine($"StartHTML:{startHTML:D10}");
    sb.AppendLine($"EndHTML:{endHTML:D10}");
    sb.AppendLine($"StartFragment:{startFragment:D10}");
    sb.AppendLine($"EndFragment:{endFragment:D10}");
    sb.Append(utf8Html);

    return sb.ToString();
  }


  /// <summary>
  /// Gets the cell value for a specific column from a grid record.
  /// </summary>
  /// <param name="column">The grid column.</param>
  /// <param name="record">The record object containing the data.</param>
  /// <returns>The formatted cell value as a string.</returns>
  public static string GetCellValue(GridColumn column, object record)
  {
    if (record is RecordEntry recordEntry)
    {
      var data = recordEntry.Data;
      if (data == null)
        return string.Empty;

      var mappingName = column.MappingName;
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
      if (column is GridNumericColumn numericColumn && value is IFormattable formattable)
      {
        var format = $"F{numericColumn.NumberDecimalDigits}";
        return formattable.ToString(format, System.Globalization.CultureInfo.CurrentCulture);
      }

      return value.ToString() ?? string.Empty;
    }

    return string.Empty;
  }

  /// <summary>
  /// Gets the inline CSS style for a row based on its background color.
  /// </summary>
  /// <param name="record">The record object containing the data.</param>
  /// <returns>CSS style string with background and foreground colors, or null if no color is defined.</returns>
  public static string? GetRowStyle(object record)
  {
    if (record is RecordEntry recordEntry)
    {
      var data = recordEntry.Data;
      if (data is IQualityNodeVM node && !string.IsNullOrEmpty(node.BackgroundColor))
      {
        string backgroundColor = node.BackgroundColor;

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
  /// <param name="grid">The SfDataGrid instance.</param>
  /// <returns>A tuple containing the background and foreground colors as HTML color strings.</returns>
  public static (string background, string foreground) GetHeaderColors(SfDataGrid grid)
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

}
