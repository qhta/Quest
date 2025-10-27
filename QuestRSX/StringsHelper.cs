using System.Globalization;
using System.Reflection;
using System.Resources;

namespace QuestRSX;

/// <summary>
/// Generic helper class for managing string resources.
/// The type parameter should be a resource class created by ResX Manager.
/// It must have a static ResourceManager property.
/// </summary>
/// <typeparam name="StringsResourcesType"></typeparam>
public class StringsHelper<StringsResourcesType> where StringsResourcesType : class
{
  /// <summary>
  /// Protected constructor to prevent direct instantiation.
  /// </summary>
  protected StringsHelper()
  {        
  }

  /// <summary>
  /// Retrieves all culture-specific variants of strings resources type embedded in the QuestRSX assembly.
  /// </summary>
  /// <returns>A dictionary where the key is the culture name and the value is another dictionary of resource keys and their translations.</returns>
  public Dictionary<string, Dictionary<string, string>> GetAllCultureSpecificVariants()
  {
    if (_AllNames == null)
    {
      var result = new Dictionary<string, Dictionary<string, string>>();
      var stringsResourcesType = typeof(StringsResourcesType);
      // Get the ResourceManager for stringsResourcesType
      var resourceManager = stringsResourcesType.GetProperty("ResourceManager", BindingFlags.Static | BindingFlags.Public)?.GetValue(null) as ResourceManager;
      if (resourceManager == null)
        throw new InvalidOperationException($"The type {stringsResourcesType.FullName} does not have a static ResourceManager property.");
      // Get the assembly containing stringsResourcesType
      var assembly = Assembly.GetAssembly(stringsResourcesType);

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
      _AllNames = result;
    }
    return _AllNames;
  }

  private Dictionary<string, Dictionary<string, string>>? _AllNames;

  /// <summary>
  /// Retrieves all available cultures for the given assembly.
  /// </summary>
  /// <param name="assembly">The assembly to inspect for satellite assemblies.</param>
  /// <returns>A list of CultureInfo objects representing the available cultures.</returns>
  private List<CultureInfo> GetAvailableCultures(Assembly assembly)
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