namespace QuestWPF.Views;
/// <summary>
/// Interaction logic for DocumentQuestView.xaml
/// </summary>
public partial class DocumentQuestView : UserControl
{
  /// <summary>
  /// Initializing constructor.
  /// </summary>
  public DocumentQuestView()
  {
    InitializeComponent();
    DataContextChanged += DocumentQualityView_DataContextChanged;
  }

  private void DocumentQualityView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
  {
    var dataContext = e.NewValue;
    if (e.NewValue is QuestItemViewModel questItemViewModel)
      dataContext = questItemViewModel.Content;
    if (dataContext is DocumentQualityVM documentQualityVM)
    {
      if (documentQualityVM.Parent is ProjectQualityVM projectQualityVM)
      {
        Resources["GradeValuesProvider"] = projectQualityVM.Scale;
      }
    }
  }
}
