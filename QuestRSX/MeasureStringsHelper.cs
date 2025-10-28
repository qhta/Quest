namespace QuestRSX;

/// <summary>
/// Class for managing string resources related to measures.
/// </summary>
public class MeasureStringsHelper: StringsHelper<MeasureStrings>  
{
  /// <summary>
  /// Protected constructor to prevent direct instantiation.
  /// </summary>
  protected MeasureStringsHelper()
  {
  }
  /// <summary>
  /// Instance of the MeasureStringsHelper singleton.
  /// </summary>
  public static MeasureStringsHelper Instance { get; } = new();

}