using Qhta.Collections;

namespace QuestRSX;

/// <summary>
/// Helper class for managing string aliases.
/// </summary>
public static class AliasHelper
{
  /// <summary>
  /// Bidirectional dictionary of aliases for names.
  /// </summary>
  public static BiDiDictionary<string, string> Aliases { get; } = new BiDiDictionary<string, string>(StringComparer.InvariantCultureIgnoreCase, StringComparer.InvariantCultureIgnoreCase)
    { 
      {"Accountability", "Reliability"},
      {"Adaptability", "Ease of adaptation"},
      {"Availability", "Ensuring availability"},
      {"Compatibility", "Ensuring compatibility"},
      {"Durability", "Ensuring durability"},
      {"Flexibility", "Ensuring flexibility"},
      {"Maintainability", "Ease of maintenance"},
      {"Modifiability", "Ease of modification"},
      {"Portability", "Ensuring portability"},
      {"Reliability", "Ensuring reliability"},
      {"Scalability", "Ease of scaling"},
      {"Sustainability", "Ensuring sustaining"},
      {"Traceability", "Ease of tracking"},
      {"Understandability", "Ease of understanding"},
      {"Usability", "Ease of use"},
      {"Verifiability", "Ease of verification"}
    };

  /// <summary>
  /// Gets the alias for a given name, if it exists.
  /// </summary>
  /// <param name="name"></param>
  /// <returns></returns>
  public static string? GetAlias(string name)
  { 
    return Aliases.TryGetValue2(name, out var alias) ? alias :
      Aliases.TryGetValue1(name, out alias) ? alias : null;
  }
}