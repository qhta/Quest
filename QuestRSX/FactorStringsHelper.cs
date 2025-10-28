namespace QuestRSX;

/// <summary>
/// Class for managing string resources related to factors.
/// </summary>
public class FactorStringsHelper: StringsHelper<FactorStrings>  
{
  /// <summary>
  /// Protected constructor to prevent direct instantiation.
  /// </summary>
  protected FactorStringsHelper()
  {
  }
  /// <summary>
  /// Gets the singleton instance of the <see cref="FactorStringsHelper"/> class.
  /// </summary>
  /// <remarks>This property provides a globally accessible instance of the <see cref="FactorStringsHelper"/>
  /// class, ensuring that only one instance is created and used throughout the application.</remarks>
  public static FactorStringsHelper Instance { get; } = new();

}