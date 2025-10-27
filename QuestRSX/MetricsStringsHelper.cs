namespace QuestRSX;

public class MetricsStringsHelper: StringsHelper<MetricsStrings>  
{
  /// <summary>
  /// Protected constructor to prevent direct instantiation.
  /// </summary>
  protected MetricsStringsHelper()
  {
  }

  public static MetricsStringsHelper Instance { get; } = new();

}