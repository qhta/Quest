namespace Quest;

/// <summary>
/// ViewModel for a quality factor assessment
/// </summary>
public class QualityMeasureVM : QualityNodeVM<QualityMeasure>, IQualityNodeVM
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
  }

  /// <summary>
  /// Access to the model's factor type
  /// </summary>
  public override QualityFactorType? FactorType
  {
    get
    {
      object? parent = Parent as IQualityNodeVM;
      while (parent is IQualityNodeVM node && parent is not QualityFactorVM)
        parent = node.Parent;
      if (parent is QualityFactorVM factorVM)
        return factorVM.FactorType;
      return null;
    }
    set { }
  }

  /// <summary>
  /// Gets the quality scale from the grandparent ProjectQualityVM
  /// </summary>
  public QualityScaleVM? Scale
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
  /// Display Name from the model
  /// </summary>
  public override string? DisplayName => Text;

  /// <summary>
  /// Gets the background color for display purposes.
  /// </summary>
  public override string? BackgroundColor
  {
    get
    {
      var colors = FactorType?.Colors?.Split(',', ';');
      if (colors == null || !colors.Any())
        return null;
      return colors[colors.Count() - 1];
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
          var gradeObject = Scale?.GetGradeByName(value);
          if (gradeObject != null)
            Value = gradeObject.Value;
        }
      }
    }
  }

  /// <summary>
  /// Evaluates the value of the node if the grade is set.
  /// </summary>
  /// <returns>double value or null if evaluation is not possible</returns>
  public override double? EvaluateValue()
  {
    if (Grade!=null)
    {
      var gradeObject = Scale?.GetGradeByName(Grade);
      if (gradeObject != null)
      {
        Value = gradeObject.Value;
        return gradeObject.Value;
      }
    } 
    return null;
  }




}
