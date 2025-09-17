namespace QuestIMP;
using Syncfusion.XlsIO;

/// <summary>
/// Commander class for handling Excel operations.
/// </summary>
public class XlsImporter
{
  private const string ProjectTitleLabel = "Ocena projektu";
  private const string WeightsFirstCellMarker = "L.p.";
  private const string QuestFirstCellMarker = "Cechy";

  /// <summary>
  /// Working workbook instance.
  /// </summary>
  public IWorkbook? Workbook { get; set; }

  /// <summary>
  /// Opens an Excel workbook from the specified file and returns the IWorkbook instance.
  /// </summary>
  /// <param name="fileName"></param>
  public void OpenWorkbook(string fileName)
  {
    ExcelEngine excelEngine = new ExcelEngine();
    IApplication application = excelEngine.Excel;
    IWorkbook workbook = application.Workbooks.Open(fileName);
    Workbook = workbook;
  }

  /// <summary>
  /// Gets information about the opened workbook, including its file name, project title, and associated worksheets.
  /// </summary>
  /// <param name="fileName"></param>
  /// <returns></returns>
  /// <exception cref="InvalidOperationException"></exception>
  public WorkbookInfo GetWorkbookInfo(string fileName)
  {
    if (Workbook == null)
    {
      throw new InvalidOperationException("Workbook is not opened.");
    }
    var info = new WorkbookInfo
    {
      FileName = fileName,
      ProjectTitle = ScanForProjectTitle(Workbook),
      Worksheets = GetWorksheets().ToArray()
    };
    return info;
  }

  /// <summary>
  /// Gets the list of worksheets. Specifies if they contain questionnaires.
  /// </summary>
  /// <returns></returns>
  /// <exception cref="InvalidOperationException"></exception>
  private IEnumerable<WorksheetInfo> GetWorksheets(IWorkbook? workbook = null)
  {
    workbook ??= Workbook ?? throw new InvalidOperationException("Workbook is not opened.");

    var resultList = new List<WorksheetInfo>();
    foreach (var worksheet in workbook.Worksheets)
    {
      var info = new WorksheetInfo
      {
        Name = worksheet.Name,
      };

      (info.HasQuest, info.QuestStart, info.QuestEnd) = ScanForTable(worksheet, QuestFirstCellMarker);
      (info.HasWeights, info.WeightsStart, info.WeightsEnd) = ScanForTable(worksheet, WeightsFirstCellMarker);
      resultList.Add(info);
    }
    return resultList;
  }

  private static (bool, string?, string?) ScanForTable(IWorksheet worksheet, string marker)
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
          var s = cell?.Value?.ToString();
          if (s == marker)
          {
            found = true;
            rangeStart = cell!.AddressLocal;
            rangeEnd = cell.AddressLocal;
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
    return (found, rangeStart, rangeEnd);
  }

  private string? ScanForProjectTitle(IWorkbook workbook)
  {
    foreach (var worksheet in workbook.Worksheets)
    {
      var projectTitle = ScanForProjectTitle(worksheet);
      if (projectTitle!=null)
        return projectTitle;
    }
    return null;
  }

  private string? ScanForProjectTitle(IWorksheet worksheet)
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
            return s;
        }
      }
      if (labelFound)
        break;
    }
    return null;
  }
}