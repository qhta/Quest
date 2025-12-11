namespace Quest;

/// <summary>
/// ViewModel for a quality metrics node
/// </summary>
public interface IQualityNodeVM : IQualityObjectVM
{

  /// <summary>
  /// Required parent view model
  /// </summary>
  public IQualityObjectVM? Parent { get; set; }

  /// <summary>
  /// Collection to which this node belongs.
  /// </summary>
  public IList? Collection { get; set; }

  /// <summary>
  /// Level of the node in the hierarchy (0 for root, 1 for factor)
  /// </summary>
  public int Level { get; }

  /// <summary>
  /// Ordering number string ended with a dot.
  /// </summary>
  public string? Numbering { get;}

  /// <summary>
  /// Text from the model
  /// </summary>
  public string? Text { get; set; }

  /// <summary>
  /// Text from the model preceding with ordering number.
  /// </summary>
  public string? TextWithNumbering { get; }

  /// <summary>
  /// Display Name from the model
  /// </summary>
  public string? DisplayName { get; }

  /// <summary>
  /// Display Name from the model preceding with ordering number.
  /// </summary>
  public string? DisplayNameWithNumbering { get; }

  /// <summary>
  /// Gets the background color name of the current element.
  /// </summary>
  public string? BackgroundColor { get; }

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

};
