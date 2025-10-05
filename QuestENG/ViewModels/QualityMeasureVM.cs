namespace Quest;

/// <summary>
/// ViewModel for a quality factor assessment
/// </summary>
public class QualityMeasureVM : ViewModel<QualityMeasure>, IQualityNodeVM
{
  /// <summary>
  /// Mandatory constructor
  /// </summary>
  /// <param name="parent">Required parent View Model</param>
  /// <param name="collection">Collection to which this node belongs.</param>
  /// <param name="model">Required data entity</param>
  public QualityMeasureVM(IQualityObjectVM parent, IList collection, QualityMeasure model) : base(model)
  {
    Parent = parent;
    Collection = collection;
    Children = new QualityNodeVMCollection(this, model.Children ?? []);  
  }

  /// <summary>
  /// Required parent view model
  /// </summary>
  public IQualityObjectVM Parent { get; set; }

  /// <summary>
  /// Gets the quality scale from the grandparent ProjectQualityVM
  /// </summary>
  public QualityScaleVM Scale
  {
    get
    {
      IQualityObjectVM? current = Parent;
      while (current != null)
      {
        if (current is ProjectQualityVM projectQualityVM)
          return projectQualityVM.Scale;
        if (current is DocumentQualityVM documentQualityVM)
          current = documentQualityVM.Parent;
        else if (current is QualityFactorVM qualityFactorVM)
          current = qualityFactorVM.Parent;
        else if (current is QualityMetricsVM qualityMetricsVM)
          current = qualityMetricsVM.Parent;
        else
          current = null;
      }
      throw new InvalidOperationException("Parent ProjectQualityVM not found in hierarchy.");
    }
  }


  /// <summary>
  /// Collection to which this node belongs.
  /// </summary>
  public IList Collection { get; set; }

  /// <summary>
  /// Gets the quality object associated with the model.
  /// </summary>
  public QualityObject QualityObject => Model;

  /// <summary>
  /// Level of the node in the hierarchy (0 for root, 1 for factor)
  /// </summary>
  public int Level => Model.Level;

  /// <summary>
  /// Ordering number string ended with a dot.
  /// </summary>
  public string? Numbering
  {
    get
    {
      var text = "";
      if (Parent is IQualityNodeVM parentNode)
        text = parentNode.Numbering;
      var number = Collection.IndexOf(this)+1;
      text += number.ToString() + ".";
      return text;
    }
  }

  /// <summary>
  /// Text from the model
  /// </summary>
  public string? Text
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
  /// Text from the model preceding with ordering number.
  /// </summary>
  public string? TextWithNumbering
  {
    [DebuggerStepThrough]
    get
    {
      var text = Model.Text;
      var numbering = Numbering;
      if (numbering != null)
        text = numbering + " " + text;
      return text;
    }
  }
  /// <summary>
  /// Weight of the value.  
  /// </summary>
  public int Weight
  {
    [DebuggerStepThrough]
    get => Model.Weight;
    set
    {
      if (Model.Weight != value)
      {
        Model.Weight = value;
        NotifyPropertyChanged(nameof(Weight));
      }
    }
  }

  /// <summary>
  /// Value of the assessment
  /// </summary>
  public double? Value
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
  /// Grade of the assessment
  /// </summary>
  public string? Grade
  {
    [DebuggerStepThrough]
    get => Model.Grade;
    set
    {
      if (Model.Grade != value)
      {
        Model.Grade = value;
        NotifyPropertyChanged(nameof(Grade));
        if (value != null)
        {
          var gradeObject = Scale.GetGradeByName(value);
          if (gradeObject != null)
            Value = gradeObject.Value;
        }
      }
    }
  }

  /// <summary>
  /// Comment added to assessment
  /// </summary>
  public string? Comment
  {
    [DebuggerStepThrough]
    get => Model.Comment;
    set
    {
      if (Model.Comment != value)
      {
        Model.Comment = value;
        NotifyPropertyChanged(nameof(Comment));
      }
    }
  }

  /// <summary>
  /// Gets the collection of child nodes associated with this node.
  /// </summary>
  /// <remarks>This property provides access to the hierarchical structure of nodes. It is read-only and cannot
  /// be null.</remarks>
  public QualityNodeVMCollection Children { get; }

  /// <summary>
  /// Evaluates the value of the node if the grade is set.
  /// </summary>
  /// <returns>double value or null if evaluation is not possible</returns>
  public double? EvaluateValue()
  {
    if (Grade!=null)
    {
      var gradeObject = Scale.GetGradeByName(Grade);
      if (gradeObject != null)
        return gradeObject.Value;
    } 
    return null;
  }

  #region Loading State Properties
  /// <summary>
  /// Determines whether the workbook is currently in a loading state.
  /// </summary>
  public bool IsLoading
  {
    [DebuggerStepThrough]
    get => _IsLoading;
    set
    {
      if (_IsLoading != value)
      {
        _IsLoading = value;
        NotifyPropertyChanged(nameof(IsLoading));
      }
    }
  }
  private bool _IsLoading;

  /// <summary>
  /// Determines the count of worksheets to load.
  /// </summary>
  public int TotalCount
  {
    [DebuggerStepThrough]
    get => _totalCount;
    set
    {
      if (_totalCount != value)
      {
        _totalCount = value;
        NotifyPropertyChanged(nameof(TotalCount));
      }
    }
  }
  private int _totalCount;

  /// <summary>
  /// Currently loaded worksheets count
  /// </summary>
  public int LoadedCount
  {
    [DebuggerStepThrough]
    get => _LoadedCount;
    set
    {
      if (_LoadedCount != value)
      {
        _LoadedCount = value;
        NotifyPropertyChanged(nameof(LoadedCount));
      }
    }
  }
  private int _LoadedCount;
  #endregion


};
