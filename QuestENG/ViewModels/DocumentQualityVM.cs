namespace Quest;

/// <summary>
/// ViewModel for a single document quality assessment
/// </summary>
public class DocumentQualityVM: ViewModel<DocumentQuality>
{
  /// <summary>
  /// Mandatory constructor
  /// </summary>
  /// <param name="model"></param>
  public DocumentQualityVM(DocumentQuality model) : base(model)
  {
    Factors = new QualityFactorVMCollection(this, model.Factors ?? []);
  }


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