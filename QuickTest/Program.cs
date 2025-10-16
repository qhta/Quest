using Polenter.Serialization;

using Quest;

namespace QuickTest;

internal class Program
{
  static void Main(string[] args)
  {
    const string inputFile = @"D:\OneDrive\Dane\Jakość\JSI2024\Z01.Quest.qxml";
    var projectQuality = (ProjectQuality)new SharpSerializer().Deserialize(inputFile);
    var outputFile = System.IO.Path.ChangeExtension(inputFile, ".xml");
    using (var writer = new StreamWriter(outputFile))
    {
      var xmlSerializer = new Qhta.Xml.Serialization.QXmlSerializer(typeof(ProjectQuality));
      xmlSerializer.Serialize(writer, projectQuality);
    }
    ProjectQuality? projectQuality2 = null;
    using (var reader = new StreamReader(outputFile))
    {
      var xmlSerializer = new Qhta.Xml.Serialization.QXmlSerializer(typeof(ProjectQuality));
      projectQuality2 = (ProjectQuality?)xmlSerializer.Deserialize(reader);
    }
    var testPassed = false;
    if (projectQuality2 != null)
    {

      string testFile = Path.ChangeExtension(inputFile, ".qxml2");
      new SharpSerializer().Serialize(projectQuality2, testFile);
      var text1 = File.ReadAllText(inputFile);
      var text2 = File.ReadAllText(testFile);
      if (text1 == text2)
      {
        testPassed = true;
        Console.WriteLine("qxml files are identical");
      }
      else
      {
        testPassed = false;
        Console.WriteLine("qxml files differ");
      }
    }

    if (projectQuality2 != null)
    {

      string testFile = Path.ChangeExtension(inputFile, ".xml2");
      using (var writer = new StreamWriter(testFile))
      {
        var xmlSerializer = new Qhta.Xml.Serialization.QXmlSerializer(typeof(ProjectQuality));
        xmlSerializer.Serialize(writer, projectQuality2);
      }
      var text1 = File.ReadAllText(outputFile);
      var text2 = File.ReadAllText(testFile);
      if (text1 == text2)
      {
        testPassed = true;
        Console.WriteLine("xml files are identical");
      }
      else
      {
        testPassed = false;
        Console.WriteLine("xml files differ");
      }
    }

    Console.WriteLine($"Test {(testPassed ? "passed" : "failed")}");
  }
}
