namespace Quest;

/// <summary>
/// Grade definition with text, numeric value, and optional meaning.
/// </summary>
public class QualityGradeVM: ViewModel<QualityGrade>
{
  /// <summary>
  /// Default constructor creating a new underlying model.
  /// </summary>
  public QualityGradeVM() : base(new QualityGrade())
  {
  }

  /// <summary>
  /// Required initialization constructor with an existing model.
  /// </summary>
  /// <param name="model"></param>
  public QualityGradeVM(QualityGrade model) : base(model)
  {
  }

  /// <summary>
  /// Text representation of the grade, e.g. "1", "2", "b.o."
  /// </summary>
  [MaxLength(4)]

  public string Text
  {
    [DebuggerStepThrough]
    get => Model.Text;
    set
    {
      if (Model.Text != value)
      {
        Model.Text = value;
        NotifyPropertyChanged(nameof(Text));
      }
    }
  }

  /// <summary>
  /// Numeric value of the grade, e.g. 1, 2, 0 (for "b.o.")
  /// </summary>
  public int Value
  {
    [DebuggerStepThrough]
    get => Model.Value;
    set
    {
      if (Model.Value != value)
      {
        Model.Value = value;
        NotifyPropertyChanged(nameof(Value));
      }
    }
  }

  /// <summary>
  /// Textual meaning of the grade.
  /// </summary>
  [MaxLength(255)]
  public string? Meaning
  {
    [DebuggerStepThrough]
    get => Model.Meaning;
    set
    {
      if (Model.Meaning != value)
      {
        Model.Meaning = value;
        NotifyPropertyChanged(nameof(Meaning));
      }
    }
  }
}