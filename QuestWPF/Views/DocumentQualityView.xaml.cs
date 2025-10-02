using QuestWPF.Helpers;

namespace QuestWPF.Views;
/// <summary>
/// Interaction logic for DocumentQualityView.xaml
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
    if ((e.NewValue as QuestItemViewModel)?.Content is DocumentQualityVM documentQualityVM)
    {
      if (documentQualityVM.Parent is ProjectQualityVM projectQualityVM)
      {
        Resources["GradeValuesProvider"] = projectQualityVM.Scale;
      }
    }
  }


}
