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
  }

  /// <summary>
  /// Document name from the model
  /// </summary>
  public string? DocumentName
  {
    [DebuggerStepThrough]
    get => Model.DocumentName;
    set
    {
      if (Model.DocumentName != value)
      {
        Model.DocumentName = value;
        NotifyPropertyChanged(nameof(DocumentName));
      }
    }
  }
}