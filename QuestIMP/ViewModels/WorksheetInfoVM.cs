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
    IsSelected = true;
  }

  /// <summary>
  /// Nameof the worksheet.
  /// </summary>
  public string? Name => Model.Name;


  /// <summary>
  /// Specifies whether the worksheet is selected for processing.
  /// </summary>
  public bool IsSelected
  {
    [DebuggerStepThrough]
    get => Model.IsSelected;
    set
    {
      if (Model.IsSelected != value)
      {
        Model.IsSelected = value;
        NotifyPropertyChanged(nameof(IsSelected));
      }
    }
  }

  /// <summary>
  /// Address range of the questionnaire in the format "Start:End", e.g. "A1:D20". Returns null if either start or end is not defined.
  /// </summary>
  public string? QuestRange
  {
    get => Model.QuestRange;
    set
    {
      if (Model.QuestRange != value)
      {
        Model.QuestRange = value;
        NotifyPropertyChanged(nameof(QuestRange));
      }
    }
  }

  /// <summary>
  /// Address range of the weights table in the format "Start:End", e.g. "A1:D20". Returns null if either start or end is not defined.
  /// </summary>
  public string? WeightsRange
  {
    get => Model.WeightsRange;
    set
    {
      if (Model.WeightsRange != value)
      {
        Model.WeightsRange = value;
        NotifyPropertyChanged(nameof(WeightsRange));
      }
    }
  }

  /// <summary>
  /// Address range of the scale table in the format "Start:End", e.g. "A1:D20". Returns null if either start or end is not defined.
  /// </summary>
  public string? ScaleRange
  {
    get => Model.ScaleRange;
    set
    {
      if (Model.ScaleRange != value)
      {
        Model.ScaleRange = value;
        NotifyPropertyChanged(nameof(ScaleRange));
      }
    }
  }

  /// <summary>
  /// Text to display.
  /// </summary>
  public string? Text
  {
    get
    {
      var result = Name;
      if (QuestRange != null)
      {
        result += $" q:({QuestRange})";
      }
      if (WeightsRange != null)
      {
        result += $" w:({WeightsRange})";
      }
      return result;
    }
  }
}