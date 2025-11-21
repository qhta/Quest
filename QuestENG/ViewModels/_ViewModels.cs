using Qhta.ObservableObjects;

namespace QuestWPF;

/// <summary>
/// Singleton class to hold all ViewModel instances for easy access.
/// </summary>
public class _ViewModels
{
  /// <summary>
  /// Private constructor to prevent external instantiation.
  /// </summary>
  private _ViewModels(){}

  /// <summary>
  /// Singleton instance of the _ViewModels class.
  /// </summary>
  public static _ViewModels Instance { get; } = new();

  /// <summary>
  /// An observable list containing all ViewModel instances.
  /// </summary>
  public ObservableList<ViewModel> All { get; } = new();

  /// <summary>
  /// Checks if there are any unsaved changes in All list.
  /// </summary>
  public bool? AreThereUnsavedChanges => All.Any(item=>item.IsChanged == true);

  ///// <summary>
  ///// Collection of all ProjectQualityVM instances.
  ///// </summary>
  //public static Dictionary<string, ProjectQualityVM> ProjectQualityVMDictionary { get; } = new();
}