using Quest;

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
    var worksheetWithScale = workbookInfo.Worksheets.FirstOrDefault(item => item.ScaleRange != null);
    if (worksheetWithScale != null)
    {
      projectQuality.Scale = WorkbookImporter.ImportScaleTable(workbook.Worksheets[worksheetWithScale.Name], worksheetWithScale);
    }
    projectQuality.DocumentQualities = new List<DocumentQuality>();
    foreach (var documentQuality in ImportDocumentQualities(workbook, workbookInfo))
    {
      projectQuality.DocumentQualities.Add(documentQuality);
    }
    return projectQuality;
  }

  /// <summary>
  /// Get the grade scale from the given worksheet.
  /// </summary>
  /// <param name="worksheet">Excel worksheet with scale table</param>  
  /// <param name="worksheetInfo">Info about worksheet with scale range recognized.</param>
  /// <returns>List of <see cref="DocumentQuality"/>. Can be empty.</returns>
  public static List<QualityGrade>? ImportScaleTable(IWorksheet worksheet, WorksheetInfo worksheetInfo)
  {
    if (worksheetInfo.ScaleRange == null) return null;
    var resultList = new List<QualityGrade>();
    var (scaleStart, scaleEnd) = WorkbookHelper.SplitRange(worksheetInfo.ScaleRange!);
    var startRowIndex = WorkbookHelper.GetCellRowIndex(scaleStart);
    var endRowIndex = WorkbookHelper.GetCellRowIndex(scaleEnd);
    var startCellIndex = WorkbookHelper.GetCellColumnIndex(scaleStart);
    for (int r = startRowIndex + 1; r <= endRowIndex; r++)
    {
      var row = worksheet.Rows[r];
      if (!row.Cells.Any()) continue; // Skip rows without cells
      if (row.Cells.Count() <= startCellIndex + 2) continue; // Skip rows without not enough cells
      var gradeId = row.Cells[startCellIndex + 0].Value;
      var gradeValueStr = row.Cells[startCellIndex + 1].Value;
      var gradeValue = (gradeValueStr == "-") ? 0 : int.Parse(gradeValueStr ?? "0");
      var gradeMeaning = row.Cells[startCellIndex + 2].Value;
      var qualityGrade = new QualityGrade { Text = gradeId, Value = gradeValue, Meaning = gradeMeaning };
      resultList.Add(qualityGrade);
    }
    return resultList;
  }

  /// <summary>
  /// Imports the list of worksheets containing questionnaires.
  /// </summary>
  /// <param name="workbook">Opened Excel workbook interface</param>  
  /// <param name="workbookInfo">Info about workbook with worksheet info collection.</param>
  /// <returns>List of <see cref="DocumentQuality"/>. Can be empty.</returns>
  private static IEnumerable<DocumentQuality> ImportDocumentQualities(IWorkbook workbook, WorkbookInfo workbookInfo)
  {
    var resultList = new List<DocumentQuality>();
    foreach (var worksheetInfo in workbookInfo.Worksheets.Where(item => item.IsSelected && item.QuestRange != null))
    {
      var worksheet = workbook.Worksheets[worksheetInfo.Name];
      var documentQuality = ImportDocumentQuality(worksheet, worksheetInfo);
      resultList.Add(documentQuality);
    }
    return resultList;
  }

  /// <summary>
  /// Asynchronously imports the list of worksheets containing questionnaires. 
  /// </summary>
  /// <param name="workbook">Opened Excel workbook interface</param>  
  /// <param name="workbookInfo">Info about workbook with worksheet info collection.</param>
  /// <returns>Asynchronously filled list of <see cref="DocumentQuality"/>. Can be empty.</returns>
  public static async IAsyncEnumerable<DocumentQuality> ImportDocumentQualitiesAsync(IWorkbook workbook, WorkbookInfo workbookInfo)
  {
    foreach (var worksheetInfo in workbookInfo.Worksheets.Where(item => item.IsSelected && item.QuestRange != null))
    {
      var worksheet = workbook.Worksheets[worksheetInfo.Name];
      var documentQuality = await ImportDocumentQualityAsync(worksheet, worksheetInfo);
      yield return documentQuality;
    }
  }

  /// <summary>
  /// Imports a single document quality object from a worksheet containing questionnaire.
  /// </summary>
  /// <param name="worksheet">Interface for opened Excel worksheet</param>
  /// <param name="worksheetInfo">Info about worksheet info.</param>
  /// <returns><see cref="DocumentQuality"/> object.</returns>
  public static DocumentQuality ImportDocumentQuality(IWorksheet worksheet, WorksheetInfo worksheetInfo)
  {
    var documentQuality = new DocumentQuality
    {
      DocumentType = worksheet.Name,
    };
    if (worksheetInfo.WeightsRange != null)
      documentQuality.DocumentTitle = GetDocumentTitle(worksheet, worksheetInfo);
    if (worksheetInfo.QuestRange != null)
      ImportQuestionnaire(worksheet, worksheetInfo, documentQuality);
    return documentQuality;
  }

  /// <summary>
  /// Asynchronously imports a single document quality object from a worksheet containing questionnaire.
  /// </summary>
  /// <param name="worksheet">Interface for opened Excel worksheet</param>
  /// <param name="worksheetInfo">Info about worksheet info.</param>
  /// <returns>Task with <see cref="DocumentQuality"/> result.</returns>
  public static async Task<DocumentQuality> ImportDocumentQualityAsync(IWorksheet worksheet, WorksheetInfo worksheetInfo)
  {
    return await Task.Run(() => ImportDocumentQuality(worksheet, worksheetInfo));
  }

  /// <summary>
  /// Imports the questionnaire table from the given worksheet into the provided document quality object.
  /// The worksheet must have a defined questionnaire range.
  /// </summary>
  /// <param name="worksheet"></param>
  /// <param name="worksheetInfo"></param>
  /// <param name="documentQuality"></param>
  /// <returns></returns>
  private static bool ImportQuestionnaire(IWorksheet worksheet, WorksheetInfo worksheetInfo, DocumentQuality documentQuality)
  {
    if (worksheetInfo.QuestRange == null) return false;
    var hasGrades = worksheetInfo.HasGrades;
    var (questStart, questEnd) = WorkbookHelper.SplitRange(worksheetInfo.QuestRange!);
    var startRowIndex = WorkbookHelper.GetCellRowIndex(questStart);
    var endRowIndex = WorkbookHelper.GetCellRowIndex(questEnd);
    var startCellIndex = WorkbookHelper.GetCellColumnIndex(questStart);
    var endCellIndex = WorkbookHelper.GetCellColumnIndex(questEnd);
    var gradesColumn = worksheetInfo.GradesColumn ?? -1;
    QualityNode? lastNode = null;
    for (int r = startRowIndex + 1; r < endRowIndex+1; r++)
    {
      var row = worksheet.Rows[r];
      if (!row.Cells.Any()) continue; // Skip rows without cells

      QualityNode? qualityNode = null;
      for (int c = startCellIndex; c <= endCellIndex; c++)
      {
        if (c>=row.Count) break; // Skip cells outside the row range
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
                if (lastNode is QualityMetricsNode lastMetricsNode)
                {
                  if (lastMetricsNode.Level < level)
                  {
                    lastMetricsNode.Children.Add(qualityMetrics);
                    qualityMetrics.Parent = lastMetricsNode;
                  }
                  else
                  {
                    while (lastMetricsNode.Level >= level && lastMetricsNode.Parent != null)
                      lastMetricsNode = lastMetricsNode.Parent;
                    lastMetricsNode.Children.Add(qualityMetrics);
                    qualityMetrics.Parent = lastMetricsNode;
                    lastNode = qualityMetrics;
                  }
                }
              }
            }
            else
              qualityNode = new QualityMeasure();
          }
          else if (c==gradesColumn && hasGrades)
          {
            if (qualityNode is QualityMeasure qualityMeasure)
              qualityMeasure.Grade = s;
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

  private static string? GetDocumentTitle(IWorksheet worksheet, WorksheetInfo worksheetInfo)
  {
    if (worksheetInfo.WeightsRange == null) return null;
    var (weightsStart, weightsEnd) = WorkbookHelper.SplitRange(worksheetInfo.WeightsRange!);
    var weightsTableRow = WorkbookHelper.GetCellRowIndex(weightsStart);
    var weightsTableCellStart = WorkbookHelper.GetCellColumnIndex(weightsStart);
    var weightsTableCellEnd = WorkbookHelper.GetCellColumnIndex(weightsEnd);
    for (int r = weightsTableRow - 1; r >= 0; r--)
    {
      var row = worksheet.Rows[r];
      if (!row.Cells.Any()) continue; // Skip rows without cells

      for (int c = weightsTableCellStart; c < int.Min(row.Count, (int)weightsTableCellEnd + 1); c++)
      {
        var cell = row.Cells[c];
        var s = cell?.Value?.ToString();
        if (!String.IsNullOrWhiteSpace(s))
          return s;
      }
    }
    return null;

  }
}