namespace Quest;

/// <summary>
/// Quality of the entire project.
/// </summary>
public class ProjectQuality: QualityObject
{
  private static readonly QualityFactorType[] _factors = 
    [
      new QualityFactorType 
        { Id = 1, Name = "Completeness", Colors = ["#FF0000","#FF6600","#FF9900","#FFCC00","#FFFF00"] },
      new QualityFactorType 
        { Id = 2, Name = "Correctness", Colors = ["#0066FF","#3399FF","#00CCFF","#00FFFF","#66FFFF"] },
      new QualityFactorType 
        { Id = 3, Name = "Consistency", Colors = ["#008000","#33CC33","#66FF66","#99FF99","#CCFFCC"] },
      new QualityFactorType 
        { Id = 4, Name = "Understandability", Colors = ["#CC00CC","#FF33FF","#FF66FF","#FF99FF","#FFCCFF"] },
      new QualityFactorType 
        { Id = 5, Name = "Modifiability", Colors = ["#009A96","#4AD5D2","#7CF3F0","#BFF9F8","#E3FFFF"] },
      new QualityFactorType 
        { Id = 6, Name = "Verifiability", Colors = ["#963634","#DA9694","#E6B8B7","#FEC1C0","#FFDEDD"] }
    ];

  ///// <summary>
  ///// Unique identifier for the entity.
  ///// </summary>
  //public int Id { get; set; }

  /// <summary>
  /// Identifies the title of the assessed project.
  /// </summary>
  [MaxLength(255)]
  public string? ProjectTitle { get; set; }

  /// <summary>
  /// Globally unique identifier for the assessed project.
  /// </summary>
  public Guid? ProjectId { get; set; }

  /// <summary>
  /// Gets or sets the scale used to evaluate quality grades.
  /// </summary>
  public QualityScale? Scale { get; set; }

  /// <summary>
  /// Gets or sets the collection of quality factors associated with the current context.
  /// </summary>
  public List<QualityFactorType>? FactorTypes { get; set; } = new List<QualityFactorType>(_factors);

  ///// <summary>
  ///// Gets or sets the collection of quality factors associated with the current context.
  ///// </summary>
  //public List<QualityFactorAggregate>? Factors { get; set; }

  ///// <summary>
  ///// A collection of phase qualities within the project.
  ///// </summary>
  //public List<PhaseQuality>? PhaseQualities { get; set; }

  /// <summary>
  /// A collection of document qualities within the project.
  /// </summary>
  public List<DocumentQuality>? DocumentQualities { get; set; }
}