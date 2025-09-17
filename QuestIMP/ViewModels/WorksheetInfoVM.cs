namespace QuestIMP;

/// <summary>
/// Represents a view model for the <see cref="WorksheetInfo"/> model, providing a binding layer between the model and
/// the user interface.
/// </summary>
/// <remarks>This class inherits from <see cref="ViewModel"/> and is specifically tailored for the <see
/// cref="WorksheetInfo"/> type. It facilitates data binding and encapsulates the logic required to interact with the
/// <see cref="WorksheetInfo"/> model.</remarks>
public class WorksheetInfoVM: ViewModel<WorksheetInfo>
{
  /// <summary>
  /// Required constructor that initializes a new instance of the <see cref="WorksheetInfoVM"/> class with the specified model.
  /// </summary>
  /// <param name="model"></param>
  public WorksheetInfoVM(WorksheetInfo model) : base(model)
  {
  }

  /// <summary>
  /// Nameof the worksheet.
  /// </summary>
  public string? Name => Model.Name;

  /// <summary>
  /// Specifies whether the worksheet contains a questionnaire.
  /// </summary>
  public bool? HasQuest => Model.HasQuest;

  /// <summary>
  /// Address range of the questionnaire in the format "Start:End", e.g. "A1:D20". Returns null if either start or end is not defined.
  /// </summary>
  public string? QuestRange => (Model.QuestStart != null && Model.QuestEnd != null) ? $"{Model.QuestStart}:{Model.QuestEnd}" : null;

  /// <summary>
  /// Specifies whether the worksheet contains a weights table.
  /// </summary>
  public bool? HasWeights => Model.HasWeights;

  /// <summary>
  /// Address range of the weights table in the format "Start:End", e.g. "A1:D20". Returns null if either start or end is not defined.
  /// </summary>
  public string? WeightsRange => (Model.WeightsStart != null && Model.WeightsEnd != null) ? $"{Model.WeightsStart}:{Model.WeightsEnd}" : null;

  /// <summary>
  /// Text to display.
  /// </summary>
  public string? Text
  {
    get
    {
      var result = Name;
      if (HasQuest == true)
      {
        result += $" q:({QuestRange})";
      }
      if (HasWeights == true)
      {
        result += $" w:({WeightsRange})";
      }
      return result;
    }
  }
}