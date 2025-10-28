namespace QuestRSX;

/// <summary>
/// Class for managing string resources related to metrics.
/// </summary>
public class MetricsStringsHelper: StringsHelper<MetricsStrings>  
{
  /// <summary>
  /// Protected constructor to prevent direct instantiation.
  /// </summary>
  protected MetricsStringsHelper()
  {
  }
  /// <summary>
  /// Instance of the MetricsStringsHelper singleton.
  /// </summary>
  public static MetricsStringsHelper Instance { get; } = new();

}