namespace QuestWPF.Views;

/// <summary>
/// View for displaying quest results in a data grid.
/// </summary>
public partial class DocumentQuestResultsView : UserControl
{
  /// <summary>
  /// Initializing constructor
  /// </summary>
  public DocumentQuestResultsView()
  {
    InitializeComponent();
    GotFocus += OnViewGotFocus;
    LostFocus += OnViewLostFocus;
    MouseDown += OnMouseDown;
  }

  private void OnViewGotFocus(object sender, RoutedEventArgs e)
  {
    FocusBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 0, 139)); // DarkBlue
  }

  private void OnViewLostFocus(object sender, RoutedEventArgs e)
  {
    FocusBorder.BorderBrush = Brushes.Gray;
  }

  private void OnMouseDown(object sender, MouseButtonEventArgs e)
  {
    this.Focus();
  }

  private void OnGotFocus(object sender, RoutedEventArgs e)
  {
    // Ensure the border updates when any child control gets focus
    FocusBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 0, 139)); // DarkBlue
  }

  /// <summary>
  /// Reference to the main window
  /// </summary>
  public MainWindow? MainWindow => Application.Current.MainWindow as MainWindow;
}
