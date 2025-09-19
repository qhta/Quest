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
  /// Specifies whether the worksheet contains a questionnaire.
  /// </summary>
  public bool HasQuest { get; set; }

  /// <summary>
  /// Address of the cell where the questionnaire starts, e.g. "A1".
  /// </summary>
  public string? QuestStart { get; set; }

  /// <summary>
  /// Address of the cell where the questionnaire ends, e.g. "D20".
  /// </summary>
  public string? QuestEnd { get; set; }

  /// <summary>
  /// Specifies whether the worksheet contains weights table for the top level metrics.
  /// </summary>
  public bool HasWeights { get; set; }

  /// <summary>
  /// Address of the cell where the weights table for the top level metrics starts.
  /// </summary>
  public string? WeightsStart { get; set; }

  /// <summary>
  /// Address of the cell where the weights table for the top level metrics ends.
  /// </summary>
  public string? WeightsEnd { get; set; }

  /// <summary>
  /// Specifies whether the worksheet should be processed.
  /// </summary>
  public bool IsSelected { get; set; }
}