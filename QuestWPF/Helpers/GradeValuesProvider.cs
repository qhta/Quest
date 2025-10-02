namespace QuestWPF.Helpers;

/// <summary>
/// Data source provider for retrieving grade values from a ProjectQualityVM instance.
/// </summary>
public class GradeValuesProvider: DataSourceProvider
{
  private ProjectQualityVM? _projectQualityVM;

  /// <summary>
  /// The ProjectQualityVM instance from which the grades will be retrieved.
  /// </summary>
  public ProjectQualityVM? ProjectQualityVM
  {
    get => _projectQualityVM;
    set
    {
      if (_projectQualityVM != value)
      {
        _projectQualityVM = value;
        OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(nameof(ProjectQualityVM)));
        Refresh(); // Refresh the data when the source changes
      }
    }
  }

  /// <summary>
  /// Starts the process of retrieving data.
  /// </summary>
  protected override void BeginQuery()
  {
    IEnumerable<QualityGradeVM> grades = _projectQualityVM?.Scale ?? (IEnumerable<QualityGradeVM>)[];
    OnQueryFinished(grades); // Notify that the data retrieval is complete
  }
}