using QuestRSX;

namespace TranslationTest;

internal class Program
{
  static void Main(string[] args)
  {
    Console.WriteLine(TranslationHelper.TranslateText("Hello, World!", "en", "pl"));
  }
}
