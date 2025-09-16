using System.Diagnostics;

namespace Quest;
using Syncfusion.XlsIO;

/// <summary>
/// Commander class for handling Excel operations.
/// </summary>
public class XlsImporter
{
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
        HasQuest = false,
        QuestStart = null,
        QuestEnd = null
      };
      // Scan for "QUEST" marker in the first column
      int maxCellNdx = 0;
      for (int r=0; r<worksheet.Rows.Count(); r++)
      {
        var row = worksheet.Rows[r];
        if (!row.Cells.Any()) continue; // Skip rows without cells

        var cell = row.Cells[0]; // First column (A)
        if (cell != null && cell.Value != null && cell.Value.ToString() == QuestFirstCellMarker)
        {
          info.HasQuest = true;
          info.QuestStart = cell.AddressLocal;
          info.QuestEnd = cell.AddressLocal;
        }
        if (info.HasQuest)
        {
          var isRowEmpty = true;
          for (int c = 0; c < row.Count; c++)
          {
            var nextCell = row.Cells[c];
            var s = nextCell?.Value?.ToString();
            if (!string.IsNullOrWhiteSpace(s))
            {
              //if (s.StartsWith("Problemy"))
              //  Debug.Assert(true);
              isRowEmpty = false;
              if (c>maxCellNdx) 
                maxCellNdx = c;
            }
          }
          if (!isRowEmpty)
          {
            // Update the end cell address
            info.QuestEnd = row.Cells[maxCellNdx].AddressLocal;
          }
          else
          {
            // Stop scanning on the first empty row after finding the start
            break;
          }
        }
      }
      resultList.Add(info);
    }
    return resultList;
  }
}