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
    Factors = new QualityFactorVMCollection(this, model.Factors ?? []);
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
}