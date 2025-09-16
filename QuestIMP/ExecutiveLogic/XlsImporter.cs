using System.Diagnostics;

namespace Quest;
using Syncfusion.XlsIO;

/// <summary>
/// Commander class for handling Excel operations.
/// </summary>
public class XlsImporter
{
  const string WeightsFirstCellMarker = "L.p.";
  const string QuestFirstCellMarker = "Cechy";

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
  /// Gets the list of worksheets. Specifies if their contain questionnaires.
  /// </summary>
  /// <returns></returns>
  /// <exception cref="InvalidOperationException"></exception>
  public IEnumerable<WorksheetInfo> GetWorksheets()
  {
    if (Workbook == null)
    {
      throw new InvalidOperationException("Workbook is not opened.");
    }
    var resultList = new List<WorksheetInfo>();
    foreach (var worksheet in Workbook.Worksheets)
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
}