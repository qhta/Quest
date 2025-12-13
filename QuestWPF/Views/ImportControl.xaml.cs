namespace QuestWPF.Views;

/// <summary>
/// Import control for starting data import operations.
/// </summary>
public partial class ImportControl : UserControl
{
  /// <summary>
  /// Initializes a new instance of the <see cref="ImportControl"/> class.
  /// </summary>
  public ImportControl()
  {
    InitializeComponent();
  }

  #region WorkbookInfo Property
  /// <summary>
  /// Dependency property for binding the <see cref="WorkbookInfo"/> to the control.
  /// </summary>
  public static readonly DependencyProperty WorkbookInfoProperty = DependencyProperty.Register(
    nameof(WorkbookInfo), 
    typeof(WorkbookInfoVM), 
    typeof(ImportControl), 
    new PropertyMetadata(null, OnWorkbookInfoChanged)
    );

  /// <summary>
  /// Reference to the workbook information view model.
  /// </summary>
  public WorkbookInfoVM WorkbookInfo
  {
    get => (WorkbookInfoVM)GetValue(WorkbookInfoProperty);
    set => SetValue(WorkbookInfoProperty, value);
  }

  /// <summary>
  /// Callback method invoked when the <see cref="WorkbookInfo"/> property changes.
  /// </summary>
  /// <param name="d">The dependency object where the property changed.</param>
  /// <param name="e">Details about the property change.</param>
  private static void OnWorkbookInfoChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (d is ImportControl control)
    {
      // Handle the property change
      var oldValue = e.OldValue as WorkbookInfoVM;
      var newValue = e.NewValue as WorkbookInfoVM;

      // Perform any necessary actions when WorkbookInfo changes
      //control.OnWorkbookInfoChanged(oldValue, newValue);
    }
  }
  #endregion

}