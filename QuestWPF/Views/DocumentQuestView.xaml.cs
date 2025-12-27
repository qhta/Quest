namespace QuestWPF.Views;

/// <summary>
/// View for displaying quest items in a tree grid.
/// </summary>
public partial class DocumentQuestView : UserControl
{
  /// <summary>
  /// Initializing constructor
  /// </summary>
  public DocumentQuestView()
  {
    InitializeComponent();
    DataContextChanged += DocumentQuestView_DataContextChanged;
    GotFocus += OnViewGotFocus;
    LostFocus += OnViewLostFocus;
    MouseDown += OnMouseDown;
  }

  private void DocumentQuestView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
  {
    var dataContext = e.NewValue;
    if (dataContext is DocumentQualityVM documentQualityVM)
    {
      if (documentQualityVM.Parent is ProjectQualityVM projectQualityVM)
      {
        if (Resources.Contains("GradeValuesProvider"))
          Resources.Remove("GradeValuesProvider");
        Resources["GradeValuesProvider"] = projectQualityVM.Scale;
      }
    }
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

}
