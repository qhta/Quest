using QuestRSX;

namespace QuickTest;

internal class Program
{
  static void Main(string[] args)
  {
    // Retrieve all culture-specific variants of FactorStrings
    var allTranslations = FactorStringsHelper.GetAllCultureSpecificVariants();

    // Print the translations
    foreach (var culture in allTranslations)
    {
      Console.WriteLine($"Culture: {culture.Key}");
      foreach (var translation in culture.Value)
      {
        Console.WriteLine($"  {translation.Key}: {translation.Value}");
      }
    }
  }
}
