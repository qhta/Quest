namespace Quest;

/// <summary>
/// ViewModel for a single document quality assessment
/// </summary>
public class DocumentQualityVM: ViewModel<DocumentQuality>, IQualityObjectVM
{
  /// <summary>
  /// Mandatory constructor
  /// </summary>
  /// <param name="parent">Required parent View Model</param>
  /// <param name="model">Required data entity</param>
  public DocumentQualityVM(IQualityObjectVM parent, DocumentQuality model) : base(model)
  {
    Parent = parent;
    Factors = new QualityFactorVMCollection(this, model.Factors?.Cast<QualityFactor>() ?? []);
  }


  /// <summary>
  /// Required parent view model
  /// </summary>
  public IQualityObjectVM Parent { get; set; }

  /// <summary>
  /// Gets the quality object associated with the model.
  /// </summary>
  public QualityObject QualityObject => Model;

  /// <summary>
  /// Document Type from the model
  /// </summary>
  public string? DocumentType
  {
    [DebuggerStepThrough]
    get => Model.DocumentType;
    set
    {
      if (Model.DocumentType != value)
      {
        Model.DocumentType = value;
        NotifyPropertyChanged(nameof(DocumentType));
      }
    }
  }

  /// <summary>
  /// Document title from the model
  /// </summary>
  public string? DocumentTitle
  {
    [DebuggerStepThrough]
    get => Model.DocumentTitle;
    set
    {
      if (Model.DocumentTitle != value)
      {
        Model.DocumentTitle = value;
        NotifyPropertyChanged(nameof(DocumentTitle));
      }
    }
  }

  /// <summary>
  /// Individual document qualities within the project
  /// </summary>
  public QualityFactorVMCollection Factors { get; set; }

  /// <summary>
  /// Value of the assessment
  /// </summary>
  public double? Value
  {
    [DebuggerStepThrough]
    get => _Value;
    set
    {
      if (_Value != value)
      {
        _Value = value;
        NotifyPropertyChanged(nameof(Value));
      }
    }
  }
  private double? _Value;

  /// <summary>
  /// Evaluates the value of the children collection.
  /// </summary>
  /// <returns>double value or null if evaluation is not possible</returns>
  public double? EvaluateValue()
  {
    if (Factors.Count != 0)
    {
      Value = Factors.EvaluateValue(true);
    }
    return Value;
  }

  /// <summary>
  /// Binds the factor types from the parent project quality to the local factors.
  /// </summary>
  /// <param name="factorTypes"></param>
  /// <exception cref="NotImplementedException"></exception>
  public void BindFactorTypes(QualityFactorTypeVMCollection factorTypes)
  {
    foreach (var factor in Factors)
    {
      if (factor.Text == null) continue;
      var allFactorStrings = FactorStringsHelper.GetAllCultureSpecificVariants();
      var name = allFactorStrings.Values.SelectMany(d => d).FirstOrDefault(kvp => kvp.Value == factor.Text).Key;
      if (name != null)
        factor.FactorType = factorTypes.FirstOrDefault(ft => ft.Model.Name == name)?.Model;
    }
  }
}