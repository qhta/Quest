using Quest;
using Syncfusion.Data;
using Syncfusion.UI.Xaml.Grid;
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
    var headerLine = new StringBuilder();
    foreach (var column in grid.Columns)
    {
      headerLine.Append(column.HeaderText).Append("\t");
    }
    lines.Add(headerLine.ToString());

    foreach (var row in grid.View.Records)
    {
      var line = new StringBuilder();
      foreach (var column in grid.Columns)
      {
        var cellValue = GetCellValue(column, row);
        line.Append(cellValue).Append("\t");
      }
      lines.Add(line.ToString());
    }

    var str = String.Join("\n", lines);
    data.SetText(str);
  }

  private static void GetDataGridAsHtml(SfDataGrid grid, DataObject data)
  {
    var html = new StringBuilder();

    // Build the HTML content
    html.AppendLine("<html>");
    html.AppendLine("<head>");
    html.AppendLine("<style>");
    html.AppendLine("table { border-collapse: collapse; font-family: Arial, sans-serif; font-size: 10pt; }");
    html.AppendLine("th { background-color: #C0C0C0; color: #404040; font-weight: bold; padding: 5px; border: 1px solid #808080; text-align: left; }");
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
      html.AppendLine("<tr>");
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
}
