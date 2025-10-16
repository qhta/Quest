namespace QuestIMP;

/// <summary>
/// Class for import Excel Workbooks.
/// </summary>
public class WorkbookImporter
{
  /// <summary>
  /// Opens an Excel workbook from the specified file and returns the IWorkbook instance.
  /// </summary>
  /// <param name="fileName">Filename of the workbook</param>
  public IWorkbook OpenWorkbook(string fileName)
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
  public ProjectQuality ImportProjectQuality(IWorkbook workbook, WorkbookInfo workbookInfo)
  {
    var projectQuality = new ProjectQuality
    {
      ProjectTitle = workbookInfo.ProjectTitle,
      DocumentQualities = new List<DocumentQuality>()
    };
    var worksheetWithScale = workbookInfo.Worksheets.FirstOrDefault(item => item.ScaleRange != null);
    if (worksheetWithScale != null)
    {
      projectQuality.Scale = ImportScaleTable(workbook.Worksheets[worksheetWithScale.Name], worksheetWithScale);
    }
    projectQuality.DocumentQualities = new List<DocumentQuality>();
    foreach (var documentQuality in ImportDocumentQualities(workbook, workbookInfo))
    {
      projectQuality.DocumentQualities.Add(documentQuality);
    }
    return projectQuality;
  }

  /// <summary>
  /// Retrieves data from workbook and populates the quality view model asynchronously.
  /// </summary>
  /// <param name="workbook">Opened Excel workbook</param>
  /// <param name="workbookInfo">Workbook info object</param>
  /// <param name="projectQuality">Project quality view model</param>
  /// <param name="callback">Method to invoke when a worksheet is imported</param>
  /// <returns></returns>
  public async Task ImportProjectQualityAsync(IWorkbook workbook, WorkbookInfo workbookInfo, ProjectQuality projectQuality, AsyncCallback? callback)
  {
    projectQuality.ProjectTitle = workbookInfo.ProjectTitle;
    var importer = new WorkbookImporter();

    var worksheetWithScale = workbookInfo.Worksheets.FirstOrDefault(item => item.ScaleRange != null);
    if (worksheetWithScale != null/* && worksheetWithScale.IsSelected*/)
    {
      var aScale = importer.ImportScaleTable(workbook.Worksheets[worksheetWithScale.Name], worksheetWithScale);
      if (aScale != null)
      {
        projectQuality.Scale = aScale;
        callback?.Invoke(new AsyncResult(true, aScale));
      }
    }

    var documentQualities = importer.ImportDocumentQualitiesAsync(workbook, workbookInfo);
    await foreach (var documentQuality in documentQualities)
    {
      //Debug.WriteLine($"Add {worksheetInfo.Name}");
      if (projectQuality.DocumentQualities == null)
        projectQuality.DocumentQualities = new List<DocumentQuality>();
      projectQuality.DocumentQualities.Add(documentQuality);
      callback?.Invoke(new AsyncResult(true, documentQuality));
    }
  }
  /// <summary>
  /// Get the grade scale from the given worksheet.
  /// </summary>
  /// <param name="worksheet">Excel worksheet with scale table</param>  
  /// <param name="worksheetInfo">Info about worksheet with scale range recognized.</param>
  /// <returns>List of <see cref="DocumentQuality"/>. Can be empty.</returns>
  public QualityScale? ImportScaleTable(IWorksheet worksheet, WorksheetInfo worksheetInfo)
  {
    if (worksheetInfo.ScaleRange == null) return null;
    var resultList = new QualityScale();
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
  private IEnumerable<DocumentQuality> ImportDocumentQualities(IWorkbook workbook, WorkbookInfo workbookInfo)
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
  private async IAsyncEnumerable<DocumentQuality> ImportDocumentQualitiesAsync(IWorkbook workbook, WorkbookInfo workbookInfo)
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
  public DocumentQuality ImportDocumentQuality(IWorksheet worksheet, WorksheetInfo worksheetInfo)
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
  public async Task<DocumentQuality> ImportDocumentQualityAsync(IWorksheet worksheet, WorksheetInfo worksheetInfo)
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
  private bool ImportQuestionnaire(IWorksheet worksheet, WorksheetInfo worksheetInfo, DocumentQuality documentQuality)
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
    for (int r = startRowIndex + 1; r < endRowIndex + 1; r++)
    {
      var row = worksheet.Rows[r];
      if (!row.Cells.Any()) continue; // Skip rows without cells

      QualityNode? currentNode = null;
      for (int c = startCellIndex; c <= endCellIndex; c++)
      {
        if (c >= row.Count) break; // Skip cells outside the row range
        var cell = row.Cells[c];
        var s = cell?.Value.Trim();
        if (!String.IsNullOrWhiteSpace(s))
        {
          if (currentNode == null)
          {
            if (Char.IsDigit(s.First()))
            {
              string? text = null;
              var ss = s.Split('.', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
              int level = ss.Length;
              if (level > 1 && !Char.IsDigit(ss.Last().FirstOrDefault()))
              {
                level--;
                text = ss.Last();
              }
              else if (Char.IsDigit(ss.Last().FirstOrDefault()) && !Char.IsDigit(ss.Last().LastOrDefault()))
              {
                level++;
                text = ss.Last();
                while (char.IsDigit(text.FirstOrDefault()))
                  text = text.Substring(1);
                text = text.Trim();
              }
              if (level == 1)
              {
                QualityFactor qualityFactor = new QualityFactor { Level = level, Text = text };
                currentNode = qualityFactor;
                documentQuality.Factors?.Add(qualityFactor);
                lastNode = qualityFactor;
              }
              else
              {
                var qualityMetrics = new QualityMetrics { Level = level, Text = text };
                currentNode = qualityMetrics;
                if (lastNode is QualityMetricsNode lastMetricsNode)
                {
                  if (lastMetricsNode.Level == level - 1)
                  {
                    lastMetricsNode.Add(qualityMetrics);
                    lastNode = qualityMetrics;
                  }
                  else
                  {
                    while (lastMetricsNode.Level >= level && lastMetricsNode.Parent != null)
                      lastMetricsNode = lastMetricsNode.Parent;
                    lastMetricsNode.Add(qualityMetrics);
                    qualityMetrics.Parent = lastMetricsNode;
                    lastNode = qualityMetrics;
                  }
                }
              }
            }
            else
            {
              var qualityMeasure = new QualityMeasure { Text = s };
              currentNode = qualityMeasure;
              if (lastNode is QualityMetricsNode lastMetricsNode)
              {
                qualityMeasure.Level = lastMetricsNode.Level + 1;
                lastMetricsNode.Add(qualityMeasure);
              }
              else
                throw new InvalidOperationException("Invalid structure of questionnaire");
            }
          }
          else if (c == gradesColumn)
          {
            if (hasGrades)
              if (currentNode is QualityMeasure qualityMeasure)
                qualityMeasure.Grade = s;
          }
          else if (c == gradesColumn + 1)
            currentNode.Comment = s;
          else if (int.TryParse(s, out int weight))
            currentNode.Weight = weight;
          else if (currentNode.Text == null)
          {
            currentNode.Text = s;
          }
        }
      }
    }
    if (worksheetInfo.WeightsRange != null)
      GetFactorWeights(worksheet, worksheetInfo, documentQuality);
    return hasGrades;
  }

  /// <summary>
  /// Gets the document title from the given worksheet by searching for the first non-empty cell over the weights table.
  /// </summary>
  /// <param name="worksheet"></param>
  /// <param name="worksheetInfo"></param>
  /// <returns></returns>
  private string? GetDocumentTitle(IWorksheet worksheet, WorksheetInfo worksheetInfo)
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

  /// <summary>
  /// Gets the factor weights from the given worksheet weights range.
  /// </summary>
  /// <param name="worksheet"></param>
  /// <param name="worksheetInfo"></param>
  /// <param name="documentQuality"></param>
  /// <returns></returns>
  private bool GetFactorWeights(IWorksheet worksheet, WorksheetInfo worksheetInfo, DocumentQuality documentQuality)
  {
    if (worksheetInfo.WeightsRange == null) return false;
    if (documentQuality.Factors == null) return false;
    if (documentQuality.Factors.Count == 0) return false;
    var (weightsStart, weightsEnd) = WorkbookHelper.SplitRange(worksheetInfo.WeightsRange!);
    var weightsTableRowStart = WorkbookHelper.GetCellRowIndex(weightsStart);
    var weightsTableRowEnd = WorkbookHelper.GetCellRowIndex(weightsEnd);
    var weightsTableCellStart = WorkbookHelper.GetCellColumnIndex(weightsStart);
    var weightsTableCellEnd = WorkbookHelper.GetCellColumnIndex(weightsEnd);
    for (int r = weightsTableRowStart + 1; r <= weightsTableRowEnd; r++)
    {
      var row = worksheet.Rows[r];
      if (!row.Cells.Any()) continue; // Skip rows without cells

      var c = weightsTableCellStart + 1;
      var text = row.Cells[c].Value;
      if (!string.IsNullOrEmpty(text))
      {
        if (text.StartsWith('='))
        {
          var refAddr = text.Substring(1);
          var refCol = WorkbookHelper.GetCellColumnIndex(refAddr);
          var refRow = WorkbookHelper.GetCellRowIndex(refAddr);
          text = worksheet.Rows[refRow].Cells[refCol].Value;
        }
        var factor = documentQuality.Factors.FirstOrDefault(f => f.Text == text);
        if (factor != null)
        {
          if (int.TryParse(row.Cells[c + 1].Value, out var weight))
          {
            factor.Weight = weight;
          }
        }
      }
    }
    return true;
  }

}