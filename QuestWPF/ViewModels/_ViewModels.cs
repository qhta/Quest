using Qhta.ObservableObjects;

namespace QuestWPF;

/// <summary>
/// Static class to hold all ViewModel instances for easy access.
/// </summary>
public static class _ViewModels
{
  /// <summary>
  /// An observable list containing all ViewModel instances.
  /// </summary>
  public static ObservableList<ViewModel> All { get; } = new();

  ///// <summary>
  ///// Collection of all ProjectQualityVM instances.
  ///// </summary>
  //public static Dictionary<string, ProjectQualityVM> ProjectQualityVMDictionary { get; } = new();
}