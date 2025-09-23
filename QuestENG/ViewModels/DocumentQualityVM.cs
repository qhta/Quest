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
    Factors = new QualityFactorCollection(this, model.Factors ?? []);
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
  public QualityFactorCollection Factors { get; set; }
}