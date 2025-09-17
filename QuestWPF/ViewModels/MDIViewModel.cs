using System.Collections.ObjectModel;

namespace QuestWPF;

/// <summary>
/// ViewModel for Multi Document Interface
/// </summary>
public class MDIViewModel: ViewModel
{
  private SwitchMode switchMode= SwitchMode.Immediate;

  /// <summary>
  /// This property enables you to control how tabs behave, with different SwitchMode values like Immediate, List, None, Quick Tabs, VS2005, and VistaFlip to provide different switching experiences. 
  /// </summary>
  public SwitchMode SwitchMode
  {
    get => switchMode;
    set
    {
      switchMode = value;
      NotifyPropertyChanged(nameof(this.SwitchMode));
    }
  }

  /// <summary>
  /// Command to change MDI layout (Cascade, Horizontal, Vertical)
  /// </summary>
  public ICommand MDILayoutChangedCommand { get; set; }

  /// <summary>
  /// Initializing constructor,
  /// </summary>
  public MDIViewModel()
  {
    MDILayoutChangedCommand = new DelegateCommand<object>(MDILayoutChanged);
  }

  private void MDILayoutChanged(object obj)
  {
    if (obj is object?[] parameters && parameters[0] is not null && parameters[1] is DockingManager dockingManager)
    {
      if (dockingManager.DocContainer is DocumentContainer documentContainer)
      {
        switch (parameters[0]!.ToString())
        {
          case "Cascade":
            documentContainer.SetLayout(MDILayout.Cascade);
            break;
          case "Horizontal":
            documentContainer.SetLayout(MDILayout.Horizontal);
            break;
          case "Vertical":
            documentContainer.SetLayout(MDILayout.Vertical);
            break;
        }
      }
    }
  }


  /// <summary>
  /// Collection of DockItems for managing dockable panels in the UI.
  /// </summary>
  public ObservableCollection<DockItem> DockCollections { get; } = new ObservableCollection<DockItem>();
}