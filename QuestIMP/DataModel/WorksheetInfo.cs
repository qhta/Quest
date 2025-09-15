namespace Quest;

/// <summary>
/// Info about a worksheet in an Excel workbook.
/// </summary>
public class WorksheetInfo
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
}