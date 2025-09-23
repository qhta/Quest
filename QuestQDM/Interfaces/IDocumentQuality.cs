namespace Quest;

/// <summary>
/// Quality of a single document.
/// </summary>
public interface IDocumentQuality
{
  /// <summary>
  /// Type of the document.
  /// </summary>
  [MaxLength(255)]
  public string? DocumentType { get; set; }

  /// <summary>
  /// Name or path of the document.
  /// </summary>
  [MaxLength(255)]
  public string? DocumentTitle { get; set; }

  /// <summary>
  /// Is the document required for the project?
  /// </summary>
  public bool IsRequired { get; set; }

  /// <summary>
  /// Is the document available for assessment?
  /// </summary>
  public bool IsAvailable { get; set; }

  /// <summary>
  /// Is the document assessed?
  /// </summary>
  public bool IsAssessed { get; set; }

  /// <summary>
  /// Gets or sets the collection of quality factors associated with the current context.
  /// </summary>
  public ICollection<IQualityFactor> Factors { get; set; }
}