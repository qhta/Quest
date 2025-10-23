using System.Xml.Serialization;

namespace Quest;

/// <summary>
/// Quality of a single document.
/// </summary>
[XmlContentProperty("Factors")]
public class DocumentQuality: QualityObject
{
  public DocumentQuality()
  {
    Factors = new QualityFactorCollection(this);
    Factors.CollectionChanged += _Factors_CollectionChanged;
  }


  /// <summary>
  /// Type of the document.
  /// </summary>
  [MaxLength(10)]
  public string? DocumentType { get; set; }

  /// <summary>
  /// Name or path of the document.
  /// </summary>
  [MaxLength(255)]
  public string? DocumentTitle { get; set; }

  /// <summary>
  /// Foreign key referencing the associated ProjectQuality.
  /// </summary>
  [XmlIgnore]
  public int ProjectQualityId { get; set; }

  /// <summary>
  /// Foreign key referencing the associated PhaseQuality.
  /// </summary>
  [MaxLength(10)]
  public string? QualityId { get; set; }

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
  public QualityFactorCollection? Factors
  {
    get => _Factors;
    set
    {
      if (_Factors != value)
      {
        _Factors = value;
        if (_Factors != null)
        {
          _Factors.Parent ??= this;
          foreach (var factor in _Factors)
            factor.DocumentQuality = this;
          _Factors.CollectionChanged += _Factors_CollectionChanged;
        }
      }
    }
  }
  private QualityFactorCollection? _Factors;

  private void _Factors_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
  {
    if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add && e.NewItems != null)
    {
      foreach (QualityFactor factor in e.NewItems)
      {
        factor.DocumentQuality = this;
      }
    }
  }

}