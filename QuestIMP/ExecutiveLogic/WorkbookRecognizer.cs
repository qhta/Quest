namespace QuestIMP;

/// <summary>
/// Class for import Excel Workbooks.
/// </summary>
public class WorkbookRecognizer: IDisposable
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
  public WorkbookRecognizer (string fileName)
  {
    ExcelEngine excelEngine = new ExcelEngine();
    IApplication application = excelEngine.Excel;
    IWorkbook workbook = application.Workbooks.Open(fileName);
    Workbook = workbook;
    FileName = fileName;
  }

  /// <summary>
  /// Must be called to close the opened workbook.
  /// </summary>
  public void Dispose()
  {
    Workbook?.Close();
  }

  private IWorkbook Workbook {  get; }
  private string FileName { get; }

  /// <summary>
  /// Gets information about the opened workbook, including its file name, project title, and associated worksheets.
  /// </summary>
  /// <returns></returns>

  public WorkbookInfo GetWorkbookInfo()
  {
    var info = new WorkbookInfo
    {
      FileName = FileName,
      ProjectTitle = ScanForProjectTitle(),
      Worksheets = GetWorksheets()
    };
    return info;
  }

  /// <summary>
  /// Gets the list of worksheets. Specifies if they contain questionnaires.
  /// </summary>
  /// <returns>List of <see cref="WorksheetInfo"/>. Can be empty.</returns>
  private List<WorksheetInfo> GetWorksheets()
  {
    var resultList = new List<WorksheetInfo>();
    foreach (var worksheet in Workbook.Worksheets)
    {
      var info = GetWorksheetInfo(worksheet.Name);
      resultList.Add(info);
    }
    return resultList;
  }

  /// <summary>
  /// Asynchronously gets the list of worksheets from previously opened workbook.
  /// </summary>
  /// <returns>Asynchronously filled list of <see cref="WorksheetInfo"/>. Can be empty.</returns>
  public async IAsyncEnumerable<WorksheetInfo> GetWorksheetsAsync()
  {
    foreach (var worksheet in Workbook.Worksheets)
    {
      var info = await GetWorksheetInfoAsync(worksheet.Name);
      yield return info;
    }
  }

  /// <summary>
  /// Gets a single worksheet info.
  /// </summary>
  ///   /// <param name="worksheetName">Name of the worksheet</param>
  /// <returns>Object of <see cref="WorksheetInfo"/></returns>
  public WorksheetInfo GetWorksheetInfo(string worksheetName)
  {
    var info = new WorksheetInfo
    {
      Name = worksheetName,
    };
    info.QuestRange = ScanForTable(worksheetName, QuestFirstCellMarker);
    info.WeightsRange = ScanForTable(worksheetName, WeightsFirstCellMarker);
    if (info.QuestRange != null) (info.HasGrades, info.GradesColumn) = CheckForGrades(worksheetName, info.QuestRange!);
    info.ScaleRange = ScanForTable(worksheetName, ScaleTableHeaders);
    return info;
  }

  /// <summary>
  /// Asynchronously gets a single worksheet info.
  /// </summary>
  /// <param name="worksheetName">Name of the worksheet</param>
  /// <returns>Task with the result of <see cref="WorksheetInfo"/> object</returns>
  public async Task<WorksheetInfo> GetWorksheetInfoAsync(string worksheetName)
  {
    return await Task.Run(() => GetWorksheetInfo(worksheetName));
  }

  /// <summary>
  /// Scans the specified worksheet for a table starting with the given marker.
  /// </summary>
  /// <param name="worksheetName">Name of the worksheet</param>
  /// <param name="marker">string content of left-top cell of the table</param>
  /// <returns>Range string or null if not found</returns>
  private string? ScanForTable(string worksheetName, string marker)
  => ScanForTable(worksheetName, [marker]);

  /// <summary>
  /// Scans the specified worksheet for a table starting with the given marker.
  /// </summary>
  /// <param name="worksheetName">Name of the worksheet</param>
  /// <param name="headers">string content of the header row</param>
  /// <returns>Range string or null if not found</returns>
  private string? ScanForTable(string worksheetName, string[] headers)
  {
    var worksheet = Workbook.Worksheets[worksheetName];
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
  /// <returns>The project title as a string if found; otherwise null</returns>
  public string? ScanForProjectTitle()
  {
    foreach (var worksheet in Workbook.Worksheets)
    {
      var projectTitle = ScanForProjectTitle(worksheet.Name);
      if (projectTitle != null)
        return projectTitle;
    }
    return null;
  }

  /// <summary>
  /// Asynchronously scans all worksheets for the project title.
  /// </summary>
  /// <returns>Task with whe project title as a string result, null if not found</returns>
  public async Task<string?> ScanForProjectTitleAsync()
  {
    return await Task.Run(() =>
    {
      foreach (var worksheet in Workbook.Worksheets)
      {
        var projectTitle = ScanForProjectTitle(worksheet.Name);
        if (projectTitle != null)
          return projectTitle;
      }
      return null;
    });
  }

  /// <summary>
  /// Scans the specified worksheet for a project title following a predefined label.
  /// </summary>
  /// <param name="worksheetName">Name of the worksheet</param>
  /// <returns>The project title as a string if found; otherwise null</returns>
  public string? ScanForProjectTitle(string worksheetName)
  {                
    var worksheet = Workbook.Worksheets[worksheetName]; 
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
  /// <param name="worksheetName">Name of the worksheet</param>
  /// <param name="questRange">Range of the top left cell with questionnaire</param>
  /// <returns>A pair of: 1. bool - true if any grade is found, otherwise false, 2. int - recognized grades column or null if not found.</returns>
  public (bool, int?) CheckForGrades(string worksheetName, string questRange)
  {
    var worksheet = Workbook.Worksheets[worksheetName];
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