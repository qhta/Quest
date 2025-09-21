using Syncfusion.Office;

namespace QuestIMP;

/// <summary>
/// Class for import Excel Workbooks.
/// </summary>
public static class WorkbookImporter
{
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
  /// Imports project quality data from the provided workbook information. Workbook must be opened prior to calling this method.
  /// </summary>
  /// <param name="workbook">Opened Excel workbook interface</param>
  /// <param name="workbookInfo">Info about workbook with worksheet info collection.</param>
  /// <returns>Project quality object</returns>
  public static ProjectQuality ImportProjectQuality(IWorkbook workbook, WorkbookInfo workbookInfo)
  {
    var projectQuality = new ProjectQuality
    {
      ProjectName = workbookInfo.ProjectTitle,
      DocumentQualities = new List<DocumentQuality>()
    };
    foreach (var worksheetInfo in workbookInfo.Worksheets)
    {
      if (worksheetInfo.HasGrades && worksheetInfo.QuestStart != null && worksheetInfo.QuestEnd != null)
      {
        var documentQuality = new DocumentQuality
        {
          DocumentTitle = worksheetInfo.Name,
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
  /// <param name="workbook">Opened Excel workbook interface</param>  
  /// <param name="workbookInfo">Info about workbook with worksheet info collection.</param>
  /// <returns>List of <see cref="DocumentQuality"/>. Can be empty.</returns>
  /// <exception cref="InvalidOperationException">Raised when workbook is null or is not opened previously.</exception>
  private static List<DocumentQuality> ImportWorksheets(IWorkbook workbook, WorkbookInfo workbookInfo)
  {
    var resultList = new List<DocumentQuality>();
    foreach (var worksheetInfo in workbookInfo.Worksheets)
    {
      var worksheet = workbook.Worksheets[worksheetInfo.Name];
      var documentQuality = ImportWorksheetInfo(worksheet, worksheetInfo);
      resultList.Add(documentQuality);
    }
    return resultList;
  }

  /// <summary>
  /// Asynchronously gets the list of worksheets from previously opened workbook. 
  /// </summary>
  /// <param name="workbook">Opened Excel workbook interface</param>  
  /// <param name="workbookInfo">Info about workbook with worksheet info collection.</param>
  /// <returns>Asynchronously filled list of <see cref="DocumentQuality"/>. Can be empty.</returns>
  public static async IAsyncEnumerable<DocumentQuality> ImportWorksheetsAsync(IWorkbook workbook, WorkbookInfo workbookInfo)
  {
    foreach (var worksheetInfo in workbookInfo.Worksheets)
    {
      var worksheet = workbook.Worksheets[worksheetInfo.Name];
      var documentQuality = await ImportWorksheetInfoAsync(worksheet, worksheetInfo);
      yield return documentQuality;
    }
  }

  /// <summary>
  /// Gets a single document quality object.
  /// </summary>
  /// <param name="worksheet">Interface for opened Excel worksheet</param>
  /// <param name="worksheetInfo">Info about worksheet info.</param>
  /// <returns><see cref="DocumentQuality"/> object.</returns>
  public static DocumentQuality ImportWorksheetInfo(IWorksheet worksheet, WorksheetInfo worksheetInfo)
  {
    var documentQuality = new DocumentQuality
    {
      DocumentTitle = worksheet.Name,
    };

    if (worksheetInfo.HasQuest)
    {
      ImportQuestTable(worksheet, worksheetInfo, documentQuality);
    }
    return documentQuality;
  }

  /// <summary>
  /// Asynchronously gets a single document quality object.
  /// </summary>
  /// <param name="worksheet">Interface for opened Excel worksheet</param>
  /// <param name="worksheetInfo">Info about worksheet info.</param>
  /// <returns>Task with <see cref="DocumentQuality"/> result.</returns>
  public static async Task<DocumentQuality> ImportWorksheetInfoAsync(IWorksheet worksheet, WorksheetInfo worksheetInfo)
  {
    return await Task.Run(() => ImportWorksheetInfo(worksheet, worksheetInfo));
  }

  private static bool ImportQuestTable(IWorksheet worksheet, WorksheetInfo worksheetInfo, DocumentQuality documentQuality)
  {
    var gradesColumn = worksheetInfo.GradesColumn;
    if (gradesColumn == null || gradesColumn < 0) return false;
    var hasGrades = worksheetInfo.HasGrades;
    var startRowIndex = WorkbookHelper.GetCellRowIndex(worksheetInfo.QuestStart!);
    var endRowIndex = WorkbookHelper.GetCellRowIndex(worksheetInfo.QuestEnd!);
    QualityNode? lastNode = null;
    for (int r = startRowIndex + 1; r < endRowIndex; r++)
    {
      var row = worksheet.Rows[r];
      if (!row.Cells.Any()) continue; // Skip rows without cells

      QualityNode? qualityNode = null;
      for (int c = 0; c < int.Min(row.Count, (int)gradesColumn); c++)
      {
        var cell = row.Cells[c];
        var s = cell?.Value?.ToString();
        if (!String.IsNullOrWhiteSpace(s))
        {
          if (qualityNode == null)
          {
            if (Char.IsDigit(s.First()))
            {
              var ss = s.Split('.', StringSplitOptions.RemoveEmptyEntries);
              int level = ss.Length;
              if (level == 1)
              {
                QualityFactor qualityFactor = new QualityFactor { Level = level };
                qualityNode = qualityFactor;
                documentQuality.Factors.Add(qualityFactor);
                qualityFactor.DocumentQuality = documentQuality;
                lastNode = qualityFactor;
              }
              else
              {
                QualityMetrics qualityMetrics = new QualityMetrics { Level = level };
                qualityNode = qualityMetrics;
                if (lastNode is QualityFactor lastFactor)
                {
                  lastFactor.Children.Add(qualityMetrics);
                  qualityMetrics.Parent = lastFactor;
                }
                if (lastNode is QualityMetricsNode lastMetrics)
                {
                  if (lastMetrics.Level<level)
                  {
                    lastMetrics.Children.Add(qualityMetrics);
                    qualityMetrics.Parent = lastMetrics;
                  }
                  else
                  {
                    while (lastMetrics.Level >= level && lastMetrics.Parent != null)
                      lastMetrics = lastMetrics.Parent;
                    lastMetrics.Children.Add(qualityMetrics);
                    qualityMetrics.Parent = lastMetrics;
                    lastNode = qualityMetrics;
                  }
                }
              }
            }
            else
              qualityNode = new QualityMeasure();
          }
          else if (int.TryParse(s, out int weight))
            qualityNode.Weight = weight;
          else
            qualityNode.Text = s;
        }
      }
    }
    return hasGrades;

  }


}