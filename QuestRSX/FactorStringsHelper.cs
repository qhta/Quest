using System.Globalization;
using System.Reflection;

namespace QuestRSX;

public static class FactorStringsHelper
{
  /// <summary>
  /// Retrieves all culture-specific variants of FactorStrings embedded in the QuestRSX assembly.
  /// </summary>
  /// <returns>A dictionary where the key is the culture name and the value is another dictionary of resource keys and their translations.</returns>
  public static Dictionary<string, Dictionary<string, string>> GetAllCultureSpecificVariants()
  {
    if (_AllFactorNames == null)
    {
      var result = new Dictionary<string, Dictionary<string, string>>();

      // Get the ResourceManager for FactorStrings
      var resourceManager = FactorStrings.ResourceManager;

      // Get the assembly containing FactorStrings
      var assembly = Assembly.GetAssembly(typeof(FactorStrings));

      if (assembly == null)
        throw new InvalidOperationException("Unable to locate the QuestRSX assembly.");

      // Get all cultures supported by the assembly
      var cultures = GetAvailableCultures(assembly);

      // Iterate through each culture and retrieve resource strings
      foreach (var culture in cultures)
      {
        var resourceSet = resourceManager.GetResourceSet(culture, true, true);
        if (resourceSet != null)
        {
          var translations = resourceSet.Cast<System.Collections.DictionaryEntry>().ToDictionary(entry => entry.Key.ToString()!, entry => entry.Value?.ToString() ?? string.Empty);

          result[culture.Name] = translations;
        }
      }
      _AllFactorNames = result;
    }
    return _AllFactorNames;
  }

  private static Dictionary<string, Dictionary<string, string>>? _AllFactorNames;

  /// <summary>
  /// Retrieves all available cultures for the given assembly.
  /// </summary>
  /// <param name="assembly">The assembly to inspect for satellite assemblies.</param>
  /// <returns>A list of CultureInfo objects representing the available cultures.</returns>
  private static List<CultureInfo> GetAvailableCultures(Assembly assembly)
  {
    var cultures = new List<CultureInfo> { CultureInfo.InvariantCulture }; // Add the invariant culture

    // Get the directory of the main assembly
    var assemblyLocation = Path.GetDirectoryName(assembly.Location);
    if (assemblyLocation == null)
      return cultures;

    // Check for satellite assemblies in subdirectories
    var satelliteDirectories = Directory.GetDirectories(assemblyLocation);
    foreach (var directory in satelliteDirectories)
    {
      var cultureName = Path.GetFileName(directory);
      try
      {
        // Attempt to load the satellite assembly for the culture
        var cultureInfo = new CultureInfo(cultureName);
        var satelliteAssemblyPath = Path.Combine(directory, $"{assembly.GetName().Name}.resources.dll");
        if (File.Exists(satelliteAssemblyPath))
        {
          cultures.Add(cultureInfo);
        }
      }
      catch (CultureNotFoundException)
      {
        // Ignore invalid culture directories
      }
    }

    return cultures;
  }
}