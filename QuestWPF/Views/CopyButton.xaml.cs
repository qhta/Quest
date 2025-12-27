using Qhta.WPF.Utils;

namespace QuestWPF.Views;
/// <summary>
/// Specialized copy to clipboard button.
/// </summary>
public partial class CopyButton : UserControl
{
  /// <summary>
  /// Initializing constructor
  /// </summary>
  public CopyButton()
  {
    InitializeComponent();
  }

  private void OnCopyButtonClick(object sender, RoutedEventArgs e)
  {
    var command = new ViewCopyCommand();
    var parameter = this.FindParent<UserControl>();
    if (command.CanExecute(parameter))
    {
      command.Execute(parameter);
    }
  }
}
