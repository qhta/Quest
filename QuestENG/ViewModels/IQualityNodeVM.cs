namespace Quest;

/// <summary>
/// ViewModel for a quality metrics node
/// </summary>
public interface IQualityNodeVM : IQualityObjectVM
{

  /// <summary>
  /// Required parent view model
  /// </summary>
  public IQualityObjectVM Parent { get; set; }

  /// <summary>
  /// Level of the factor in the hierarchy (0 for root, 1 for factor)
  /// </summary>
  public int Level { get; }

  /// <summary>
  /// Factor text from the model
  /// </summary>
  public string? Text { get; set; }

  /// <summary>
  /// Weight of the factor  
  /// </summary>
  public int Weight { get; set; }

  /// <summary>
  /// Value of the assessment
  /// </summary>
  public double? Value { get; set; }

  /// <summary>
  /// Comment added to assessment
  /// </summary>
  public string? Comment { get; set; }

  /// <summary>
  /// Gets the collection of child nodes associated with this node.
  /// </summary>
  public QualityNodeVMCollection Children { get; }

  /// <summary>
  /// Evaluates the value of the node.
  /// </summary>
  /// <returns>double value or null if evaluation is not possible</returns>
  public double? EvaluateValue();

  #region Loading State Properties
  /// <summary>
  /// Determines whether the workbook is currently in a loading state.
  /// </summary>
  public bool IsLoading { get; set; }

  /// <summary>
  /// Determines the count of worksheets to load.
  /// </summary>
  public int TotalCount { get; set; }

  /// <summary>
  /// Currently loaded worksheets count
  /// </summary>
  public int LoadedCount { get; set; }
  #endregion
};
