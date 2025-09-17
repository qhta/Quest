namespace QuestIMP;

/// <summary>
/// Observable collection of <see cref="WorksheetInfoVM"/> objects.
/// </summary>
public class WorksheetInfoCollection : ObservableList<WorksheetInfoVM>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="WorksheetInfoCollection"/> class with a list of <see cref="WorksheetInfo"/>.
  /// </summary>
  /// <param name="items"></param>
  public WorksheetInfoCollection(IEnumerable<WorksheetInfo> items) : base(items.Select(item => new WorksheetInfoVM(item)))
  {
  }
}