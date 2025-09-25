namespace QuestIMP;

/// <summary>
/// Information about a worksheet in an Excel workbook with details about questionnaires and weights.
/// </summary>
public record WorksheetInfo
{
  /// <summary>
  /// Name of the worksheet.
  /// </summary>
  public string? Name { get; set; }

  /// <summary>
  /// Range of the questionnaire, e.g. "A20:L200".
  /// </summary>
  public string? QuestRange { get; set; }

  /// <summary>
  /// Range of the weights table for the top level metrics, e.g. "D7:H15".
  /// </summary>
  public string? WeightsRange { get; set; }

  /// <summary>
  /// Index of the column with grades in the questionnaire. Null if column not found.
  /// </summary>
  public int? GradesColumn { get; set; }

  /// <summary>
  /// Specifies whether the has grades associated with the questionnaire.
  /// </summary>
  public bool HasGrades { get; set; }

  /// <summary>
  /// Range of the scale table, e.g. "B21:D32".
  /// </summary>
  public string? ScaleRange { get; set; }

  /// <summary>
  /// Specifies whether the worksheet should be processed.
  /// </summary>
  public bool IsSelected { get; set; }
}