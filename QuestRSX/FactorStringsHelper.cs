namespace QuestRSX;

public class FactorStringsHelper: StringsHelper<FactorStrings>  
{
  /// <summary>
  /// Protected constructor to prevent direct instantiation.
  /// </summary>
  protected FactorStringsHelper()
  {
  }

  public static FactorStringsHelper Instance { get; } = new();

}