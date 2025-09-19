using Quest;

namespace QuestIMP;
using System.Collections.Generic;

using Syncfusion.XlsIO;

/// <summary>
/// Commander class for handling Excel operations.
/// </summary>
public class XlsImporter
{
  private const string ProjectTitleLabel = "Ocena projektu";
  private const string WeightsFirstCellMarker = "L.p.";
  private const string QuestFirstCellMarker = "Cechy";
  private const string GradesColumnHeader = "Ocena";

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
  /// Imports project quality data from the provided workbook information.
  /// </summary>
  /// <param name="workbookInfo"></param>
  /// <returns></returns>
  public ProjectQuality ImportProjectQuality(WorkbookInfo workbookInfo)
  {
    var projectQuality = new ProjectQuality
    {
      ProjectName = workbookInfo.ProjectTitle,
      DocumentQualities = new List<DocumentQuality>()
    };
    foreach (var worksheetInfo in workbookInfo.Worksheets)
    {
      if (worksheetInfo.IsSelected && worksheetInfo.QuestStart != null && worksheetInfo.QuestEnd != null)
      {
        var documentQuality = new DocumentQuality
        {
          DocumentName = worksheetInfo.Name,
        };
        // Here you would add logic to read the actual questionnaire data
        // and populate the QualityFactors list.
        projectQuality.DocumentQualities.Add(documentQuality);
      }
    }
    return projectQuality;
  }

  /// <summary>
  /// Gets the list of worksheets. Specifies if they contain questionnaires.
  /// </summary>
  /// <param name="workbook">Interface for the Excel worksheet. If null, then previously opened Workbook is used.</param>
  /// <returns>List of <see cref="WorksheetInfo"/>. Can be empty.</returns>
  /// <exception cref="InvalidOperationException">Raised when workbook is null and not opened previously.</exception>
  private List<WorksheetInfo> GetWorksheets(IWorkbook? workbook = null)
  {
    workbook ??= Workbook ?? throw new InvalidOperationException("Workbook is not opened.");
    var resultList = new List<WorksheetInfo>();
    foreach (var worksheet in workbook.Worksheets)
    {
      var info = GetWorksheetInfo(worksheet);
      resultList.Add(info);
    }
    return resultList;
  }

  /// <summary>
  /// Asynchronously gets the list of worksheets from previously opened workbook. Fills the provided collection.
  /// </summary>
  /// <returns>Asynchronously filled list of <see cref="WorksheetInfo"/>. Can be empty.</returns>
  /// <exception cref="InvalidOperationException">Raised when workbook is null and not opened previously.</exception>
  public async IAsyncEnumerable<WorksheetInfo> GetWorksheetsAsync()
  {
    var workbook = Workbook ?? throw new InvalidOperationException("Workbook is not opened.");
    foreach (var worksheet in workbook.Worksheets)
    {
      var info = await GetWorksheetInfoAsync(worksheet);
      //Thread.Sleep(1000);
      yield return info;
    }
  }

  /// <summary>
  /// Gets a single worksheet info.
  /// </summary>
  /// <returns></returns>
  /// <exception cref="InvalidOperationException"></exception>
  public WorksheetInfo GetWorksheetInfo(IWorksheet worksheet)
  {
    var info = new WorksheetInfo
    {
      Name = worksheet.Name,
    };

    (info.HasQuest, info.QuestStart, info.QuestEnd) = ScanForTable(worksheet, QuestFirstCellMarker);
    (info.HasWeights, info.WeightsStart, info.WeightsEnd) = ScanForTable(worksheet, WeightsFirstCellMarker);
    if (info.HasQuest)
    {
      info.IsSelected = CheckForGrades(worksheet, info.QuestStart!, info.QuestEnd!);
    }
    return info;
  }

  /// <summary>
  /// Gets a single worksheet info.
  /// </summary>
  /// <returns></returns>
  /// <exception cref="InvalidOperationException"></exception>
  public async Task<WorksheetInfo> GetWorksheetInfoAsync(IWorksheet worksheet)
  {
    return await Task.Run(() => GetWorksheetInfo(worksheet));
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

  /// <summary>
  /// Scans all worksheets for the project title.
  /// </summary>
  /// <param name="workbook">Interface for the Excel worksheet. If null, then previously opened Workbook is used.</param>
  /// <returns>The project title as a string if found; otherwise null</returns>
  /// <exception cref="InvalidOperationException">Raised when workbook is null and not opened previously.</exception>
  public string? ScanForProjectTitle(IWorkbook? workbook = null)
  {
    workbook ??= Workbook ?? throw new InvalidOperationException("Workbook is not opened.");
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
  /// <param name="workbook">Interface for the Excel worksheet.  If null, then previously opened Workbook is used.</param>
  /// <returns>The project title as a string if found; otherwise null</returns>
  /// <exception cref="InvalidOperationException">Raised when workbook is null and not opened previously.</exception>  
  public async Task<string?> ScanForProjectTitleAsync(IWorkbook? workbook = null)
  {
    workbook ??= Workbook ?? throw new InvalidOperationException("Workbook is not opened.");
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
  public string? ScanForProjectTitle(IWorksheet worksheet)
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

  /// <summary>
  /// Checks the worksheet for the presence of grades in the specified range.
  /// </summary>
  /// <param name="worksheet">Interface for the Excel worksheet</param>
  /// <param name="questStart">Local address of the top left cell with questionnaire</param>
  /// <param name="questEnd">Local address of the top left cell with questionnaire</param>
  /// <returns>True if any grade is found, otherwise false</returns>
  public bool CheckForGrades(IWorksheet worksheet, string questStart, string questEnd)
  {
    var gradesColumn = -1;
    var gradesFound = false;
    var startRow = int.Parse(new String(questStart.Where(Char.IsDigit).ToArray())) - 1;
    var endRow = int.Parse(new String(questEnd.Where(Char.IsDigit).ToArray())) - 1;
    for (int r = startRow; r < endRow; r++)
    {
      var row = worksheet.Rows[r];
      if (!row.Cells.Any()) continue; // Skip rows without cells

      if (gradesColumn < 0)
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
        var cell = row.Cells[gradesColumn];
        var s = cell?.Value?.ToString();
        if (!String.IsNullOrWhiteSpace(s) && !s.StartsWith("="))
        {
          gradesFound = true;
          break;
        }
      }
    }
    return gradesFound;
  }
}