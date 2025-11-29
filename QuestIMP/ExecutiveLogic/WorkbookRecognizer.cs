namespace QuestIMP;

/// <summary>
/// Class for import Excel Workbooks.
/// </summary>
public static class WorkbookRecognizer
{
  private const string ProjectTitleLabel = "Ocena projektu";
  private const string ProjectTitleDefaultLabel = "Tytuł projektu";
  private static readonly string[] ScaleTableHeaders = ["Ocena", "Wartość", "Znaczenie"];
  private const string WeightsFirstCellMarker = "L.p.";
  private const string QuestFirstCellMarker = "Cechy";
  private const string GradesColumnHeader = "Ocena";

  /// <summary>
  /// Opens an Excel workbook from the specified file and returns the IWorkbook instance.
  /// </summary>
  /// <param name="fileName">Filename of the workbook</param>
  public static IWorkbook OpenWorkbook(string fileName)
  {
    ExcelEngine excelEngine = new ExcelEngine();
    IApplication application = excelEngine.Excel;
    IWorkbook workbook = application.Workbooks.Open(fileName);
    return workbook;
  }

  /// <summary>
  /// Gets information about the opened workbook, including its file name, project title, and associated worksheets.
  /// </summary>
  /// <param name="workbook">Opened Excel workbook interface</param>
  /// <param name="fileName">Filename of the workbook</param>
  /// <returns></returns>

  public static WorkbookInfo GetWorkbookInfo(IWorkbook workbook, string fileName)
  {
    var info = new WorkbookInfo
    {
      FileName = fileName,
      ProjectTitle = ScanForProjectTitle(workbook),
      Worksheets = GetWorksheets(workbook)
    };
    return info;
  }

  /// <summary>
  /// Gets the list of worksheets. Specifies if they contain questionnaires.
  /// </summary>
  /// <param name="workbook">Opened Excel workbook interface</param>
  /// <returns>List of <see cref="WorksheetInfo"/>. Can be empty.</returns>
  private static List<WorksheetInfo> GetWorksheets(IWorkbook workbook)
  {
    var resultList = new List<WorksheetInfo>();
    foreach (var worksheet in workbook.Worksheets)
    {
      var info = GetWorksheetInfo(worksheet);
      resultList.Add(info);
    }
    return resultList;
  }

  /// <summary>
  /// Asynchronously gets the list of worksheets from previously opened workbook.
  /// </summary>
  /// <param name="workbook">Opened Excel workbook interface</param>
  /// <returns>Asynchronously filled list of <see cref="WorksheetInfo"/>. Can be empty.</returns>
  public static async IAsyncEnumerable<WorksheetInfo> GetWorksheetsAsync(IWorkbook workbook)
  {
    foreach (var worksheet in workbook.Worksheets)
    {
      var info = await GetWorksheetInfoAsync(worksheet);
      yield return info;
    }
  }

  /// <summary>
  /// Gets a single worksheet info.
  /// </summary>
  /// <param name="worksheet">Opened Excel worksheet interface</param>
  /// <returns>Object of <see cref="WorksheetInfo"/></returns>
  public static WorksheetInfo GetWorksheetInfo(IWorksheet worksheet)
  {
    var info = new WorksheetInfo
    {
      Name = worksheet.Name,
    };
    info.QuestRange = ScanForTable(worksheet, QuestFirstCellMarker);
    info.WeightsRange = ScanForTable(worksheet, WeightsFirstCellMarker);
    if (info.QuestRange != null) (info.HasGrades, info.GradesColumn) = CheckForGrades(worksheet, info.QuestRange!);
    info.ScaleRange = ScanForTable(worksheet, ScaleTableHeaders);
    return info;
  }

  /// <summary>
  /// Asynchronously gets a single worksheet info.
  /// </summary>
  /// <param name="worksheet">Opened Excel worksheet interface</param>
  /// <returns>Task with the result of <see cref="WorksheetInfo"/> object</returns>
  public static async Task<WorksheetInfo> GetWorksheetInfoAsync(IWorksheet worksheet)
  {
    return await Task.Run(() => GetWorksheetInfo(worksheet));
  }

  /// <summary>
  /// Scans the specified worksheet for a table starting with the given marker.
  /// </summary>
  /// <param name="worksheet">Excel worksheet interface</param>
  /// <param name="marker">string content of left-top cell of the table</param>
  /// <returns>Range string or null if not found</returns>
  private static string? ScanForTable(IWorksheet worksheet, string marker)
  => ScanForTable(worksheet, [marker]);

  /// <summary>
  /// Scans the specified worksheet for a table starting with the given marker.
  /// </summary>
  /// <param name="worksheet">Excel worksheet interface</param>
  /// <param name="headers">string content of the header row</param>
  /// <returns>Range string or null if not found</returns>
  private static string? ScanForTable(IWorksheet worksheet, string[] headers)
  {
    var found = false;
    string? rangeStart = null;
    string? rangeEnd = null;
    int maxCellNdx = 0;
    for (int r = 0; r < worksheet.Rows.Count(); r++)
    {
      var row = worksheet.Rows[r];
      if (!row.Cells.Any()) continue; // Skip rows without cells
      if (!found)
      {
        for (int c = 0; c < row.Count; c++)
        {
          var cell = row.Cells[c];
          var s = cell.Value;
          if (s == headers[0])
          {
            found = true;
            for (int h = 1; h < headers.Length; h++)
            {
              if (c + h >= row.Count || row.Cells[c + h].Value != headers[h])
              {
                found = false;
                break;
              }
            }
            if (found)
            {
              rangeStart = cell.AddressLocal;
              rangeEnd = cell.AddressLocal;
            }
            break;
          }
        }
      }
      if (found)
      {
        var isRowEmpty = true;
        for (int c = 0; c < row.Count; c++)
        {
          var cell = row.Cells[c];
          var s = cell?.Value?.ToString();
          if (!string.IsNullOrWhiteSpace(s))
          {
            isRowEmpty = false;
            if (c > maxCellNdx)
              maxCellNdx = c;
          }
        }
        if (!isRowEmpty)
        {
          // Update the end cell address
          rangeEnd = row.Cells[maxCellNdx].AddressLocal;
        }
        else
        {
          // Stop scanning on the first empty row after finding the start
          break;
        }
      }
    }
    if (found)
      return rangeStart + ":" + rangeEnd;
    else
      return null;
  }

  /// <summary>
  /// Scans all worksheets for the project title.
  /// </summary>
  /// <param name="workbook">Interface for the Excel worksheet.</param>
  /// <returns>The project title as a string if found; otherwise null</returns>
  public static string? ScanForProjectTitle(IWorkbook workbook)
  {
    foreach (var worksheet in workbook.Worksheets)
    {
      var projectTitle = ScanForProjectTitle(worksheet);
      if (projectTitle != null)
        return projectTitle;
    }
    return null;
  }

  /// <summary>
  /// Asynchronously scans all worksheets for the project title.
  /// </summary>
  /// <param name="workbook">Interface for the Excel worksheet.</param>
  /// <returns>Task with whe project title as a string result, null if not found</returns>
  public static async Task<string?> ScanForProjectTitleAsync(IWorkbook workbook)
  {
    return await Task.Run(() =>
    {
      foreach (var worksheet in workbook.Worksheets)
      {
        var projectTitle = ScanForProjectTitle(worksheet);
        if (projectTitle != null)
          return projectTitle;
      }
      return null;
    });
  }

  /// <summary>
  /// Scans the specified worksheet for a project title following a predefined label.
  /// </summary>
  /// <param name="worksheet">Interface for the Excel worksheet</param>
  /// <returns>The project title as a string if found; otherwise null</returns>
  public static string? ScanForProjectTitle(IWorksheet worksheet)
  {
    var labelFound = false;
    for (int r = 0; r < worksheet.Rows.Count(); r++)
    {
      var row = worksheet.Rows[r];
      if (!row.Cells.Any()) continue; // Skip rows without cells

      for (int c = 0; c < row.Count; c++)
      {
        var cell = row.Cells[c];
        var s = cell?.Value?.ToString();
        if (s == ProjectTitleLabel)
        {
          labelFound = true;
          continue;
        }
        if (labelFound)
        {
          if (!String.IsNullOrWhiteSpace(s))
          {
            if (s == ProjectTitleDefaultLabel && c < row.Count() - 1)
            {
              // Check next row in the same column but next row
              s = worksheet.Rows[r + 1].Cells[c]?.Value?.ToString();
            }
            if (!String.IsNullOrWhiteSpace(s))
              return s;
            else
              return null;
          }
        }
      }
      if (labelFound)
        break;
    }
    return null;
  }

  /// <summary>
  /// Checks the worksheet for the presence of grades in the specified range.
  /// </summary>
  /// <param name="worksheet">Interface for the Excel worksheet</param>
  /// <param name="questRange">Range of the top left cell with questionnaire</param>
  /// <returns>A pair of: 1. bool - true if any grade is found, otherwise false, 2. int - recognized grades column or null if not found.</returns>
  public static (bool, int?) CheckForGrades(IWorksheet worksheet, string questRange)
  {
    int? gradesColumn = null;
    var gradesFound = false;
    var (questStart, questEnd) = WorkbookHelper.SplitRange(questRange);
    var startRowIndex = WorkbookHelper.GetCellRowIndex(questStart);
    var endRowIndex = WorkbookHelper.GetCellRowIndex(questEnd);
    for (int r = startRowIndex; r < endRowIndex; r++)
    {
      var row = worksheet.Rows[r];
      if (!row.Cells.Any()) continue; // Skip rows without cells

      if (gradesColumn == null)
      {
        for (int c = 0; c < row.Count; c++)
        {
          var cell = row.Cells[c];
          var s = cell?.Value?.ToString();
          if (s == GradesColumnHeader)
          {
            gradesColumn = c;
            break;
          }
        }
      }
      else
      {
        var cell = row.Cells[(int)gradesColumn];
        var s = cell?.Value?.ToString();
        if (!String.IsNullOrWhiteSpace(s) && !s.StartsWith("="))
        {
          gradesFound = true;
          break;
        }
      }
    }
    return (gradesFound, gradesColumn);
  }

}