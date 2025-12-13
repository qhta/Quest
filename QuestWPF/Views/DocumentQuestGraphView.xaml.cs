namespace QuestWPF.Views;
/// <summary>
/// Document quest graph view. Contains a graph for visualizing results of the document's factors evaluation.
/// </summary>
public partial class DocumentQuestGraphView : UserControl
{
  /// <summary>
  /// Initializing constructor.
  /// </summary>
  public DocumentQuestGraphView()
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
