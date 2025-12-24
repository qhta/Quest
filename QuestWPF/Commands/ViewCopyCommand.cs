using Quest;
using Syncfusion.Data;
using Syncfusion.UI.Xaml.Grid;
using ColorConverter = System.Windows.Media.ColorConverter;
using GridColumn = Syncfusion.UI.Xaml.Grid.GridColumn;

namespace QuestWPF;

/// <summary>
/// Command to copy current view to clipboard.
/// </summary>
public class ViewCopyCommand : Command
{
  /// <summary>
  /// Determines whether the command can execute with the specified parameter.
  /// </summary>
  public override bool CanExecute(object? parameter)
  {
    if (parameter is ViewCopyData ViewCopyData)
      parameter = ViewCopyData.ActiveView;

    if (parameter is DocumentQuestView)
      return true;
    if (parameter is DocumentQuestResultsView)
      return true;

    if (parameter is QuestView questView)
    {
      var projectQualityVM = questView.DataContext as ProjectQualityVM;
      return projectQualityVM != null;
    }
    return false;
  }

  /// <summary>
  /// A method to execute the command. The parameter should be a reference to current QuestView.
  /// </summary>
  /// <param name="parameter"></param>
  public override void Execute(object? parameter)
  {
    if (parameter is ViewCopyData ViewCopyData)
      parameter = ViewCopyData.ActiveView;

    try
    {
      if (parameter is DocumentQuestView documentQuestView)
      {
        if (documentQuestView.DataContext is DocumentQualityVM documentQualityVm)
          CopyDocumentQuality(documentQualityVm);
      }
      else if (parameter is DocumentQuestResultsView documentQuestResultsView)
      {
        CopyDocumentQualityResults(documentQuestResultsView);
      }
      else if (parameter is QuestView questView)
      {
        if (questView.DataContext is ProjectQualityVM projectQualityVM)
        {
          if (questView.ModelTreeView.SelectedItem is DocumentQualityVM documentQualityVM)
          {
            CopyDocumentQuality(documentQualityVM);
          }
          else
            CopyProjectQuality(projectQualityVM);
        }
      }
    }
    catch (Exception e)
    {
      MessageBox.Show(e.Message);
    }
  }


  /// <summary>
  /// Copies the current ProjectQualityVM to clipboard as  serializable and as XML text.
  /// </summary>
  /// <returns></returns>
  public void CopyProjectQuality(ProjectQualityVM projectQualityVM)
  {
    Task.Run(() => CopyProjectQualityAsync(projectQualityVM));
  }

  private async Task CopyProjectQualityAsync(ProjectQualityVM projectQualityVM)
  {
    ProjectQuality? model = projectQualityVM.Model;
    try
    {
      var bytes = await FileCommandHelper.SerializeProjectQualityAsync(model);
      Clipboard.SetData(DataFormats.Serializable, bytes);
      var memoryStream = new MemoryStream(bytes);
      using (var reader = new StreamReader(memoryStream))
      {
        var str = await reader.ReadToEndAsync();
        Clipboard.SetText(str);
      }

    }
    catch (Exception e)
    {
      MessageBox.Show(e.Message);
    }
  }

  /// <summary>
  /// Copies the current DocumentQuestView to clipboard as XML text.
  /// </summary>
  /// <returns></returns>
  public void CopyDocumentQuality(DocumentQualityVM documentQualityVM)
  {
    Task.Run(() => CopyDocumentQualityAsync(documentQualityVM));
  }

  private async Task CopyDocumentQualityAsync(DocumentQualityVM documentQualityVM)
  {
    DocumentQuality? model = documentQualityVM.Model;
    try
    {
      var bytes = await FileCommandHelper.SerializeDocumentQualityAsync(model);
      Clipboard.SetData(DataFormats.Serializable, bytes);
      var memoryStream = new MemoryStream(bytes);
      using (var reader = new StreamReader(memoryStream))
      {
        var str = await reader.ReadToEndAsync();
        Clipboard.SetText(str);
      }

    }
    catch (Exception e)
    {
      MessageBox.Show(e.Message);
    }
  }

  /// <summary>
  /// Copies the current DocumentQuestView to clipboard as XML text.
  /// </summary>
  /// <returns></returns>
  public void CopyDocumentQualityResults(DocumentQuestResultsView documentQuestResultsView)
  {
    try
    {
      var grid = documentQuestResultsView.DocumentQuestResultsGrid;
      DataObject dataObject = new();
      GetDataGridImage(grid, dataObject);
      GetDataGridAsText(grid, dataObject);
      GetDataGridAsHtml(grid, dataObject);
      Clipboard.SetDataObject(dataObject, true);

    }
    catch (Exception e)
    {
      MessageBox.Show(e.Message);
    }
  }

  private static void GetDataGridImage(FrameworkElement element, DataObject data)
  {
    double width = element.ActualWidth;
    double height = element.ActualHeight;
    Debug.WriteLine($"Copying image of size {width}x{height}");
    RenderTargetBitmap bmpCopied = new RenderTargetBitmap
      ((int)Math.Round(width), (int)Math.Round(height), 96, 96, PixelFormats.Default);
    DrawingVisual dv = new DrawingVisual();
    using (DrawingContext dc = dv.RenderOpen())
    {
      dc.DrawRectangle(Brushes.White, null, new Rect(new Point(), new Size(width, height)));
      var vb = new VisualBrush(element);
      dc.DrawRectangle(vb, null, new Rect(new Point(), new Size(width, height)));
    }
    bmpCopied.Render(dv);
    data.SetImage(bmpCopied);
  }

  private static void GetDataGridAsText(SfDataGrid grid, DataObject data)
  {
    var lines = new List<string>();
    var line = new List<string>();
    foreach (var column in grid.Columns)
    {
      line.Add(column.HeaderText);
    }
    lines.Add(String.Join("\t",line));
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

  private static void GetDataGridAsHtml(SfDataGrid grid, DataObject data)
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
    File.WriteAllText("clipboard.html", htmlClipboardFormat);
    // Set HTML data in clipboard
    data.SetData(DataFormats.Html, htmlClipboardFormat);
  }

  /// <summary>
  /// Converts HTML content to the HTML Clipboard Format required by Microsoft Office applications.
  /// </summary>
  /// <param name="html">The HTML content to convert.</param>
  /// <returns>HTML content with proper clipboard format headers.</returns>
  private static string ConvertToHtmlClipboardFormat(string html)
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
  private static string GetCellValue(GridColumn column, object record)
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
  private static string? GetRowStyle(object record)
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
  private static bool IsColorDark(Color color)
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
  private static (string background, string foreground) GetHeaderColors(SfDataGrid grid)
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
  private static T? FindVisualChild<T>(DependencyObject? parent) where T : DependencyObject
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
  private static string ColorToHex(Color color)
  {
    return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
  }
}
