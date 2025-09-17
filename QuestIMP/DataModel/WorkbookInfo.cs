namespace Quest;

/// <summary>
/// Information about a workbook, including its file name, project title, and associated worksheets.
/// </summary>
public record WorkbookInfo
{
  /// <summary>
  /// File name of the workbook.
  /// </summary>
  public string? FileName { get; set; }

  /// <summary>
  /// Project title associated with the workbook.
  /// </summary>
  public string? ProjectTitle { get; set; }

  /// <summary>
  /// Information about the worksheets contained in the workbook.
  /// </summary>
  public WorksheetInfo[] Worksheets { get; set; } = [];
}