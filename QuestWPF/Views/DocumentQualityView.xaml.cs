namespace QuestWPF.Views;
/// <summary>
/// View of the document quality.
/// Consists of a DocumentQuestView, DocumentQuestResultsView and DocumentQuestGraphView.
/// </summary>
public partial class DocumentQualityView : UserControl
{
  /// <summary>
  /// Initializing constructor
  /// </summary>
  public DocumentQualityView()
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
